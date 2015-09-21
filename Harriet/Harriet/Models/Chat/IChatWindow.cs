using System;
using System.ComponentModel;

namespace Harriet.Models.Chat
{
    /// <summary>指示に従って描画対象を生成する、モデルとしてのチャットウィンドウを定義します。</summary>
    public interface IChatWindowModel : INotifyPropertyChanged, IDisposable
    {
        //TODO?: このインターフェースはIOがごっちゃになってるのが若干良くなく、構造的にしたければ
        //・モデルからの入力である関数Hide, Render(Text/Content)のインターフェース
        //・ビューモデルとのやり取りとしてのContent, Closing, HideRequested, Flush
        //を別のインターフェースにした方がいい


        /// <summary>文字を描画します。</summary>
        /// <param name="text">描画したい文字</param>
        /// <param name="charPerSec">1秒あたりの表示文字ペース(0以下なら即時実行)</param>
        void RenderText(string text, double charPerSec);

        /// <summary> 
        /// 任意のコンテンツを表示させます。
        /// 基本的にUIElementが渡される事が想定されています
        ///  </summary>
        /// <param name="content"> 表示させたいもの </param>
        void RenderContent(object content);

        /// <summary>描画途中のテキストがある場合、それを一気に最後まで表示します。</summary>
        void Flush();

        /// <summary>ウィンドウを隠すよう要求します。</summary>
        void Hide();

        /// <summary>
        /// 描画内容を取得します。
        /// </summary>
        object Content { get; }

        /// <summary>位置とサイズを取得します。</summary>
        ChatWindowPositionModel Position { get; }

        /// <summary>ウィンドウを閉じる時に発生します。</summary>
        event EventHandler Closing;

        /// <summary>ウィンドウを隠す必要があるときに発生します。</summary>
        event EventHandler HideRequested;

    }



}
