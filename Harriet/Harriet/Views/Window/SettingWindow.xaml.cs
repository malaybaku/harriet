
namespace Harriet.Views
{
    /// <summary>Harrietの設定画面を表します。</summary>
    public partial class SettingWindow
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public SettingWindow()
        {
            InitializeComponent();

            MainWindow.Current.Closed += (_, __) => this.Close();
        }
    }
}