using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace Harriet.Views
{
    /// <summary>
    /// RectColorChoosableButton.xaml の相互作用ロジック
    /// </summary>
    public partial class RectColorChoosableButton : Button
    {
        /// <summary>インスタンスを初期化します。</summary>
        public RectColorChoosableButton()
        {
            InitializeComponent();
        }

        /// <summary>ボタンが押されたとき色選択ダイアログを表示し、前面色を再設定します。</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //1:Windows Formsの色ダイアログで召喚
            //2:ダイアログ結果っぽいものからsenderに色情報を渡す

            var cDialog = new Forms.ColorDialog {FullOpen = true};
            var result = cDialog.ShowDialog();
            if (result != Forms.DialogResult.OK)
            {
                return;
            }

            var brush = new SolidColorBrush(
                Color.FromArgb(
                    cDialog.Color.A,
                    cDialog.Color.R,
                    cDialog.Color.G,
                    cDialog.Color.B
                    )
                );
            SetValue(ForegroundProperty, brush);

        }

    }
}
