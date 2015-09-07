using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

using Livet;

using Harriet.Models.Core;

namespace Harriet.Models.Voice
{
    /// <summary>音声合成と再生を担当するオブジェクトを表します。</summary>
    public class VoiceOperator : NotificationObject, IVoiceOperator
    {
        /// <summary>音声合成の担当インスタンスと設定をもとにインスタンスを初期化します。</summary>
        /// <param name="synther">音声合成の担当インスタンス</param>
        /// <param name="setting">発声の設定</param>
        public VoiceOperator(IVoiceSynther synther, IVoiceSetting setting)
        {
            _synther = synther;
            VoiceSetting = setting;
        }

        /// <summary> 
        /// バイト配列として読みだした音を再生します。
        /// ブロッキングで実行され、音声再生が終了するかStop関数が呼び出されるかすると
        /// 関数が終了します。
        /// </summary>
        public async Task PlayByPronounce(string pronounce, bool useLipSynch)
        {
            Stop();

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            var data = _synther.CreateWav(pronounce);
            //ガードが必要: 合成音声に失敗した場合を想定する
            if (data == null || data.Length == 0) return;

            using (var ms = new MemoryStream(data))
            using (var sp = new SoundPlayer(ms))
            {
                try
                {
                    //ヘッダから再生時間を読み取る
                    int timeMillSec = (int) (new WaveInfo(data).PlayTime*1000.0);

                    if (useLipSynch)
                    {
                        RequestLipSynchByWavData(data);                        
                    }
                    sp.Play();

                    await Task.Delay(timeMillSec, token);
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

                //TODO: 現状コレだとVolumeもSpeedも反映出来ないことになる。何か対策しなさい
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
                    player.Volume = Math.Min(Math.Max(VoiceSetting.Volume * 0.01, 0.0), 1.0);
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

        private readonly IVoiceSynther _synther;
        private CancellationTokenSource _cts;

        IVoiceSetting VoiceSetting { get; }

        /// <summary>リップシンク開始を通知する。</summary>
        /// <param name="data">wavの波形データ</param>
        private void RequestLipSynchByWavData(byte[] data)
        {
            var lipSynchValues = WaveInfo.VolumesDiscrete(data, ModelCore.TimerUpdateIntervalSec);
            var lipsynch = new LipSyncher(DateTime.Now, lipSynchValues, ModelCore.TimerUpdateIntervalSec);
            LipSynchRequested?.Invoke(this, new LipSynchEventArgs(lipsynch));
        }

    }



}
