using System;

using Harriet.CharacterInterface;
using Harriet.Models.Core;
using Harriet.Models.Chat;
using Harriet.ViewModels;
using Harriet.Models.BasicUtil;

using HarrietModelInterface;

namespace Harriet.Models.Scripting
{
    /// <summary> スクリプトへ公開するAPIを定義します。 </summary>
    public interface IScriptApi : IDisposable
    {
        #region 関数

        #region 会話やチャットウィンドウと関係が深いもの

        /// <summary> 発音記号投げてAquesTalkで喋らせる</summary>
        /// <param name="pronounce"> AquesTalkの発音記号列 </param>
        /// <param name="text"> 平文のテキスト </param>
        /// <param name="letterPerSec"> 1秒間あたりの表示文字数(0以下なら即時表示) </param>
        /// <param name="useLipSynch"> リップシンクを使うかどうか </param>
        void Say(string pronounce, string text, double letterPerSec, bool useLipSynch);

        /// <summary> 声なしでテキストだけ表示 </summary>
        /// <param name="text"> 表示するテキスト </param>
        /// <param name="letterPerSec"> 1秒間あたりの表示文字数(0以下なら即時表示) </param>
        void Text(string text, double letterPerSec);

        /// <summary> 声 + リップシンクだけ </summary>
        /// <param name="pronounce"> 発音を表す文字列 </param>
        /// <param name="useLipSynch"> リップシンクを使うかどうか </param>
        void Voice(string pronounce, bool useLipSynch);

        /// <summary>指定された音声ファイルを用いて口パクつき再生を行います。</summary>
        /// <param name="wavpath">音声ファイルへのパス</param>
        /// <param name="useLipSynch">リップシンクを使うかどうか</param>
        void PlaySound(string wavpath, bool useLipSynch);
        
        /// <summary>音声ファイルで音を再生し、テキストも同時に指定します。</summary>
        /// <param name="wavpath">音声ファイルのパス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="letterPerSec">1秒あたりの文字進み速度(0以下の場合一気に書き切る)</param>
        /// <param name="useLipSynch">リップシンクを使うかどうか</param>
        void PlaySoundWithText(string wavpath, string text, double letterPerSec, bool useLipSynch);

        /// <summary> 任意のコンテンツを表示させる </summary>
        /// <param name="content"> 表示するコンテンツ </param>
        void ShowContent(object content);

        #endregion

        #region 移動に関するモノ

        /// <summary> ある場所を目標地点にしてそこまで移動する </summary>
        /// <param name="x"> 目標地点のX座標 </param>
        /// <param name="y"> 目標地点のY座標 </param>
        void Go(double x, double y);

        /// <summary> ある場所まで瞬時にワープする </summary>
        /// <param name="x"> 目標地点のX座標 </param>
        /// <param name="y"> 目標地点のY座標 </param>
        void Warp(double x, double y);

        /// <summary> 移動を停止する </summary>
        void PauseMove();

        /// <summary> 移動を再開する </summary>
        void ResumeMove();

        /// <summary> その場で動きを止めて落ち着く </summary>
        void StopMove();

        #endregion

        #region その他

        /// <summary>チャット枠を隠します。</summary>
        void HideChatWindow();

        /// <summary> 一定時間待機する </summary>
        /// <param name="durationSec"> 待機時間 </param>
        void Wait(double durationSec);

        /// <summary>UIスレッド上で何かの操作を実行します。</summary>
        /// <param name="action">実行したい処理</param>
        void Invoke(Action action);

        #endregion

        #endregion 

        #region プロパティ

        /// <summary>表示中のキャラクターを取得します。</summary>
        IHarrietCharacter Character { get; }
        /// <summary>キャラクターを表示しているウィンドウを取得します。</summary>
        IMainWindow Window { get; }

        /// <summary>チャット枠を取得します。</summary>
        IChatWindowModel ChatWindow { get; }
        /// <summary>任意スクリプトの実行要求が出来るインターフェースを取得します。</summary>
        IScriptRequestor ScriptRequest { get; }
        /// <summary>キャラクターの名前を取得します。</summary>
        string CharacterName { get; }

        ///// <summary>合成音声の発音前処理を表します。</summary>
        //ITextToPronounceConverter TextConverter { get; set; }

        /// <summary>設定事項を取得します。</summary>
        SettingWindowViewModel Setting { get; }

        #endregion

        #region イベント

        /// <summary>キーボードが押下あるいは押下解除されると発生します。</summary>
        event EventHandler<KeyboardHookedEventArgs> KeyboardUpDown;

        #endregion

    }
}
