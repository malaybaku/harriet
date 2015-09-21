using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

using Livet;

using Harriet.Models.Core;
using HarrietModelInterface;
using System.Collections.Generic;
using System.Linq;

namespace Harriet.Models.Voice
{
    /// <summary>音声合成と再生を担当するオブジェクトを表します。</summary>
    public class VoiceOperator : NotificationObject, IVoiceOperator
    {
        /// <summary>音声合成の担当インスタンスと設定をもとにインスタンスを初期化します。</summary>
        /// <param name="synther">音声合成の担当インスタンス</param>
        /// <param name="setting">発声の設定</param>
        public VoiceOperator(IVoiceSetting setting)
        {
            _setting = setting;

            AvailableVoices = VoiceSynthesizerLoader.LoadAvailableVoices();
            AvailableTextConverters = TextToPronounceConverterLoader.LoadAvailableTextConverters();

            _currentSynthesizerName = setting.VoiceType;
            _textToPronounceConverterName = setting.TextConverterType;

        }

        /// <summary> 
        /// バイト配列として読みだした音を再生します。
        /// ブロッキングで実行され、音声再生が終了するかStop関数が呼び出されるかすると
        /// 関数が終了します。
        /// </summary>
        public async Task PlayByPronounce(string text, bool useLipSynch)
        {
            Stop();

            UpdateBySetting();

            string pronounce = _textToPronounceConverter.Convert(text);

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            var data = _currentSynthesizer.CreateWav(pronounce);
            //ガードが必要: 合成音声に失敗した場合を想定する
            if (data == null || data.Length == 0) return;

            using (var ms = new MemoryStream(data))
            using (var sp = new SoundPlayer(ms))
            {
                try
                {
                    //再生時間を読み取る
                    int durationMillisec = (int)WaveInfoWithNAudio.GetDurationMillisec(data);

                    if (useLipSynch)
                    {
                        RequestLipSynchByWavData(data);                        
                    }
                    sp.Play();

                    await Task.Delay(durationMillisec, token);
                }
                catch (TaskCanceledException)
                {
                    sp.Stop();
                }
            }
        }

        /// <summary>
        /// 音声ファイルを指定して発声を行います。
        /// </summary>
        /// <param name="wavpath">音声ファイルへのパス</param>
        /// <param name="useLipSynch">リップシンクを使うかどうか</param>
        /// <returns></returns>
        public async Task PlayByFile(string wavpath, bool useLipSynch)
        {
            Stop();

            if (!File.Exists(wavpath)) return;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            MediaPlayer player = null;
            DispatcherHelper.UIDispatcher.Invoke(() => player = new MediaPlayer());
            try
            {
                var uri = new Uri(wavpath, UriKind.Absolute);

                //NOTE:こっから先ではSpeedを反映しないが、これはライセンス上wavのスピード変えられるのが問題あるケースへの対処。
                DispatcherHelper.UIDispatcher.Invoke(() => player.Open(uri));
                while(!DispatcherHelper.UIDispatcher.Invoke(() => player.NaturalDuration.HasTimeSpan))
                {
                    //待機時間は小さな適当な値であればOK
                    await Task.Delay(30);
                }
                int timeMillSec = (int) DispatcherHelper.UIDispatcher.Invoke(() => 
                    player.NaturalDuration.TimeSpan.TotalMilliseconds);
                DispatcherHelper.UIDispatcher.Invoke(() =>
                {
                    //[0, 1]の区間におさえる
                    player.Volume = Math.Min(Math.Max(_setting.Volume * 0.01, 0.0), 1.0);
                });

                if (Path.GetExtension(wavpath) == ".wav" && useLipSynch)
                {
                    //wavの場合だけ簡単に波形解析が通るのでLipSynchする
                    byte[] data = File.ReadAllBytes(wavpath);
                    RequestLipSynchByWavData(data);
                }

                DispatcherHelper.UIDispatcher.Invoke(() => player.Play());

                await Task.Delay(timeMillSec, token);
            }
            catch (TaskCanceledException)
            {
                DispatcherHelper.UIDispatcher.Invoke(() =>  player.Stop());
            }
            catch (UriFormatException)
            {
                //存在しないファイルを指定した場合の挙動: めんどいので無視！
            }

        }

        /// <summary>音声を再生中である場合、再生を中止します。</summary>
        public void Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        /// <summary>VoiceOperatorのリソースを解放します。</summary>
        public void Dispose() => Stop();

        /// <summary>リップシンクが必要になると発生します。</summary>
        public event EventHandler<LipSynchEventArgs> LipSynchRequested;

        private CancellationTokenSource _cts;

        #region 音声合成器
        private IVoiceSynthesize _currentSynthesizer;
        private string _currentSynthesizerName;

        public readonly IReadOnlyList<IVoiceSynthesizeFactory> AvailableVoices;

        /// <summary>
        /// 指定された名前の合成音声器を利用しようとします。
        /// 不正な名前が指定された場合は何もしません。
        /// </summary>
        /// <param name="name">合成音声器の名前</param>
        private void UpdateSynthesizer(string name)
        {
            if (!AvailableVoices.Any(v => v.Name == name)) return;

            if(_currentSynthesizerName != name)
            {
                _currentSynthesizerName = name;

                _currentSynthesizer?.Dispose();
                _currentSynthesizer = null;
            }
        }
        #endregion
       
        #region テキスト変換器
        private ITextToPronounceConverter _textToPronounceConverter;
        private string _textToPronounceConverterName;

        private readonly IReadOnlyDictionary<string, ITextToPronounceConverterFactory> AvailableTextConverters;

        private void UpdateTextConverter(string name)
        {
            if (!AvailableTextConverters.ContainsKey(name)) return;

            if(_textToPronounceConverterName != name)
            {
                _textToPronounceConverterName = name;

                _textToPronounceConverter?.Dispose();
                _textToPronounceConverter = null;
            }
        }
        #endregion

        private readonly IVoiceSetting _setting;

        /// <summary>発声の前に設定を反映し、合成音声器やテキスト変換器が必要な場合それらを準備します。</summary>
        private void UpdateBySetting()
        {
            UpdateSynthesizer(_setting.VoiceType);
            if (_currentSynthesizer == null)
            {
                _currentSynthesizer = AvailableVoices
                    .Single(s => s.Name == _currentSynthesizerName)
                    .CreateVoiceSynthesizer();
            }
            _currentSynthesizer.Volume = _setting.Volume;
            _currentSynthesizer.Speed = _setting.Speed;
            _currentSynthesizer.Pitch = _setting.Pitch;

            UpdateTextConverter(_setting.TextConverterType);
            if (_textToPronounceConverter == null)
            {
                _textToPronounceConverter = AvailableTextConverters[_textToPronounceConverterName].CreateConverter();
            }
        }


        /// <summary>リップシンク開始を通知する。</summary>
        /// <param name="data">wavの波形データ</param>
        private void RequestLipSynchByWavData(byte[] data)
        {
            //DEBUG: NAudioベースの実装に差し替えられるか様子見
            var lipSynchValues = WaveInfoWithNAudio.GetVolumesDiscrete(data, ModelCore.TimerUpdateIntervalSec);

            //var lipSynchValues = WaveInfo.GetVolumesDiscrete(data, ModelCore.TimerUpdateIntervalSec);
            var lipsynch = new LipSyncher(DateTime.Now, lipSynchValues, ModelCore.TimerUpdateIntervalSec);
            LipSynchRequested?.Invoke(this, new LipSynchEventArgs(lipsynch));
        }

    }



}
