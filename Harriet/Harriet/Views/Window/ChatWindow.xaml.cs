using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Harriet.Views
{
    /// <summary>会話を表示するウィンドウです。</summary>
    public partial class ChatWindow : Window
    {
        /// <summary>インスタンスを初期化します。</summary>
        public ChatWindow()
        {
            InitializeComponent();
            MainWindow.Current.Closed += (_, __) => Close();
        }

        /// <summary>
        /// ドラッグ移動を許可します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

    }
}