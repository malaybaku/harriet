using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using System.Windows;

using Livet;

using Harriet.Models.Core;
using Harriet.Models.Chat;
using Harriet.Models.Voice;
using Harriet.Models.BasicUtil;
using Harriet.CharacterInterface;
using Harriet.ViewModels;
using HarrietModelInterface;

namespace Harriet.Models.Scripting
{
    /// <summary> キャラ等のインスタンスに関連付けられたスクリプト用のAPIを提供します。 </summary>
    public class ScriptApi : IScriptApi
    {
        /// <summary>APIの処理を実際に担当できるサブモジュールを用いてインスタンスを初期化します。</summary>
        /// <param name="window">キャラ表示ウィンドウ</param>
        /// <param name="character">実際のキャラクター</param>
        /// <param name="voiceOperator">発声処理器</param>
        /// <param name="chatWindow">チャット枠</param>
        /// <param name="requestor">スクリプト実行要求器</param>
        /// <param name="setting">設定事項</param>
        /// <param name="characterName">キャラクター名</param>
        public ScriptApi(
            IMainWindow window,
            IHarrietCharacter character,
            IVoiceOperator voiceOperator,
            IChatWindowModel chatWindow,
            IScriptRequestor requestor,
            CharacterSetting setting,
            IScriptApiSetting scriptApiSetting,
            string characterName
            )
        {
            this.Window = window;
            this.Character = character;
            this._voiceOperater = voiceOperator;
            this.ChatWindow = chatWindow;
            this.CharacterName = characterName;
            this.Setting = new SettingWindowViewModel(setting);
            this.ScriptRequest = requestor;

            _keyboardHook = new KeyboardHook(OnKeyboardUpDown);

            ////プラグインがあったら拾い、無かったら無視
            //try
            //{
            //    TextConverter = TextToPronounceConverterLoader.Load().FirstOrDefault() ??
            //                    new ImeTextConverter(); 
            //}
            //catch(Exception)
            //{
            //    TextConverter = new ImeTextConverter();
            //}

            _scriptApiSetting = scriptApiSetting;
        }

        #region インターフェース実装

        /// <summary> 発音記号投げてAquesTalkで喋らせる</summary>
        /// <param name="input"> AquesTalkの発音記号列 </param>
        /// <param name="text"> 平文のテキスト </param>
        /// <param name="letterPerSec"> 1秒間あたりの表示文字数(0以下なら即時表示) </param>
        /// <param name="useLipSynch"> リップシンクを使うかどうか </param>
        public void Say(string input, string text, double letterPerSec = 20.0, bool useLipSynch = true)
        {
            if (IsOnUIThread) return;

            //string pronounce = TextConverter.Convert(input);

            var t1 = Task.Run(() => ChatWindow.RenderText(text, letterPerSec));
            var t2 = _voiceOperater.PlayByPronounce(input, useLipSynch);

            Task.WaitAll(t1, t2);
            Wait(_scriptApiSetting.SerihuInterval);
        }

        /// <summary> 声なしでテキストだけ表示 </summary>
        /// <param name="text"> 表示するテキスト </param>
        /// <param name="letterPerSec"> 1秒間あたりの表示文字数(0以下なら即時表示) </param>
        public void Text(string text, double letterPerSec = 20.0)
        {
            if (IsOnUIThread) return;

            ChatWindow.RenderText(text, letterPerSec);
        }

        /// <summary> 声 + リップシンクだけ </summary>
        /// <param name="input"> 発音を表す文字列 </param>
        /// <param name="useLipSynch"> リップシンクを使うかどうか </param>
        public void Voice(string input, bool useLipSynch = true)
        {
            if (IsOnUIThread) return;

            //string pronounce = TextConverter.Convert(input);
            _voiceOperater.PlayByPronounce(input, useLipSynch).Wait();
            Wait(_scriptApiSetting.SerihuInterval);
        }

        /// <summary>指定された音声ファイルを用いて口パクつき再生を行います。</summary>
        /// <param name="wavpath">音声ファイルへのパス</param>
        /// <param name="useLipSynch">リップシンクを使うかどうか</param>
        public void PlaySound(string wavpath, bool useLipSynch = true)
        {
            if (IsOnUIThread) return;

            _voiceOperater.PlayByFile(wavpath, useLipSynch).Wait();
        }

        /// <summary>音声ファイルで音を再生し、テキストも同時に指定します。</summary>
        /// <param name="wavpath">音声ファイルのパス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="letterPerSec">1秒あたりの文字進み速度(0以下の場合一気に書き切る)</param>
        /// <param name="useLipSynch">リップシンクを使うかどうか</param>
        public void PlaySoundWithText(string wavpath, string text, double letterPerSec, bool useLipSynch)
        {
            throw new NotSupportedException("Harriet does not yet support PlaySoundWithText function in this version");
        }

        /// <summary> 任意のコンテンツを表示させる </summary>
        /// <param name="content"> 表示するコンテンツ </param>
        public void ShowContent(object content) => ChatWindow.RenderContent(content);

        /// <summary> ある場所を目標地点にしてそこまで移動する </summary>
        /// <param name="x"> 目標地点のX座標 </param>
        /// <param name="y"> 目標地点のY座標 </param>
        public void Go(double x, double y)
        {
            //TODO: ずりずり移動する方法を実装すること
            Warp(x, y);
        }

        /// <summary> ある場所まで瞬時にワープする </summary>
        /// <param name="x"> 目標地点のX座標 </param>
        /// <param name="y"> 目標地点のY座標 </param>
        public void Warp(double x, double y)
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                Window.CenterX = x;
                Window.CenterY = y;
            });
        }

        /// <summary> 移動を停止する </summary>
        public void PauseMove()
        {

        }

        /// <summary>移動を再開する</summary>
        public void ResumeMove()
        {

        }

        /// <summary>移動を完全に止める</summary>
        public void StopMove()
        {

        }

        /// <summary>チャット枠を隠す</summary>
        public void HideChatWindow() => ChatWindow.Hide();

        /// <summary>指定された秒数だけ待機します。</summary>
        /// <param name="durationSec">待機する時間(秒)</param>
        public void Wait(double durationSec)
        {
            if (IsOnUIThread) return;

            Task.Delay(TimeSpan.FromSeconds(durationSec)).Wait();
        }

        /// <summary>UIスレッド上で何かの操作を実行します。</summary>
        /// <param name="action">実行したい処理</param>
        public void Invoke(Action action)
        {
            try
            {
                DispatcherHelper.UIDispatcher.Invoke(action);
            }
            catch (Exception ex)
            {
                //握りつぶしつつ最低限の通知出すスタイル
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>キャラクターそのものを取得します。</summary>
        public IHarrietCharacter Character { get; }

        /// <summary>APIの親に相当するウィンドウ的なものを取得します。</summary>
        public IMainWindow Window { get; }

        /// <summary>チャット枠(モデル版)</summary>
        public IChatWindowModel ChatWindow { get; }

        /// <summary>キャラクターの名前を取得します。</summary>
        public string CharacterName { get; }

        /// <summary>キャラに固有の設定事項を取得します。</summary>
        public SettingWindowViewModel Setting { get; }

        /// <summary>スクリプトの実行リクエスト</summary>
        public IScriptRequestor ScriptRequest { get; }

        //private ITextToPronounceConverter _textConverter;
        ///// <summary>音声合成の前処理器。実行中に変更してもOKなタイプ</summary>
        //public ITextToPronounceConverter TextConverter
        //{
        //    get
        //    {
        //        return _textConverter ?? (_textConverter = new ImeTextConverter());
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _textConverter = value;
        //        }
        //    }
        //}

        /// <summary>リソースを解放します。</summary>
        public void Dispose()
        {
            ChatWindow.Dispose();
            _voiceOperater.Dispose();
            _keyboardHook.Dispose();
        }

        /// <summary>キーボードが押下あるいは押下解除されると発生します。</summary>
        public event EventHandler<KeyboardHookedEventArgs> KeyboardUpDown;

        #endregion

        /// <summary>実行コンテキストがUIスレッドであるかどうかを取得</summary>
        private bool IsOnUIThread => (
            Thread.CurrentThread.ManagedThreadId ==
            DispatcherHelper.UIDispatcher.Thread.ManagedThreadId);

        /// <summary>発声の処理を実際に担う</summary>
        private readonly IVoiceOperator _voiceOperater;
        
        /// <summary>タイピングの監視によりリッチなアプリケーションが云々。</summary>
        private readonly KeyboardHook _keyboardHook;

        /// <summary>APIの挙動設定</summary>
        private readonly IScriptApiSetting _scriptApiSetting;

        //横流しするだけ
        private void OnKeyboardUpDown(object sender, KeyboardHookedEventArgs e)
        {
            try
            {
                this.KeyboardUpDown?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when executing KeyboardUpDown event:" + ex.Message);
            }
        }


    }

}
