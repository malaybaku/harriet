using System;
using System.ComponentModel;

namespace Harriet.Models.Core
{
    /// <summary> ウィンドウどうしの位置決めに使う為に位置を公開するインターフェース </summary>
    public interface IMainWindow : INotifyPropertyChanged
    {
        /// <summary>キャラの中心位置のX座標を取得、設定します。</summary>
        double CenterX { get; set; }
        /// <summary>キャラの中心位置のY座標を取得、設定します。</summary>
        double CenterY { get; set; }

        /// <summary>ウィンドウの左端座標を取得、設定します。</summary>
        double Left { get; set; }
        /// <summary>ウィンドウの右端座標を取得します。</summary>
        double Right { get; }
        /// <summary>ウィンドウの上端座標を取得、設定します。</summary>
        double Top { get; set; }
        /// <summary>ウィンドウの下端座標を取得します。</summary>
        double Bottom { get; }

        /// <summary>ウィンドウの幅を取得、設定します。</summary>
        double Width { get; set; }
        /// <summary>ウィンドウの高さを取得、設定します。</summary>
        double Height { get; set; }

        /// <summary>ウィンドウを最前面に表示するかを取得、設定します。</summary>
        bool Topmost { get; }

        /// <summary>ウィンドウを閉じる操作が有効かを取得、設定します。</summary>
        bool CanClose { get; set; }

        /// <summary>ウィンドウに表示する内容を設定します。</summary>
        object Content { set; }

        /// <summary>ウィンドウを閉じます。</summary>
        void Close();

        /// <summary>
        /// ウィンドウを閉じてほしいという要求が起きたときに発生します。
        /// 受け取った側が適切な処理の後にClose関数を呼び出すことが想定されています。
        /// </summary>
        event EventHandler CloseRequested;

        /// <summary> ウィンドウが閉じた時に発生します。 </summary>
        event EventHandler Closing;
    }

}
