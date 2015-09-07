using System.Windows;
using System.Windows.Input;

namespace Harriet.Views
{    
    /// <summary>Harrietのメインウィンドウを表します。このウィンドウは一切表示されません。</summary>
    public partial class MainWindow
    {
        /// <summary>現在表示されているMainWindowのインスタンスを取得します。</summary>
        public static MainWindow Current { get; private set; }
        
        /// <summary>インスタンスを初期化します。</summary>
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            WindowItself = this;
        }

        /// <summary> 掴んで動かす操作を認める。 </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        /// <summary> キャラを撫でたのを検知 </summary>
        /// <param name="e">  </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var p = e.GetPosition(this);

            Nadenade += (p - _point).Length;
            _point = p;
        }

        private Point _point;


        #region 添付プロパティとしての追加プロパティ実装

        #region 禁忌を犯す: スクリプトAPIへの便宜をはかるのにどうしてもWindowを見せたいので添付プロパティにする

        /// <summary>ウィンドウ自身を添付プロパティとして定義します。</summary>
        public static readonly DependencyProperty WindowItselfProperty =
            DependencyProperty.Register("WindowItself", typeof(Window), typeof(Window), new PropertyMetadata(null));

        /// <summary>ウィンドウ自身を取得します。</summary>
        public Window WindowItself
        {
            get { return (Window)GetValue(WindowItselfProperty); }
            set { SetValue(WindowItselfProperty, value); }
        }

        /// <summary>
        /// ウィンドウ自身を取得します。
        /// </summary>
        /// <param name="target">取得元</param>
        /// <returns>ウィンドウそのもの</returns>
        public static MainWindow GetWindowItself(Window target)
        {
            return (MainWindow)target.GetValue(WindowItselfProperty);
        }
 
        /// <summary>ウィンドウの参照を設定します。</summary>
        /// <param name="target">設定対象</param>
        /// <param name="value">設定したいウィンドウ</param>
        public static void SetWindowItself(Window target, Window value)
        {
            target.SetValue(WindowItselfProperty, value);
        }

        #endregion

        #region CharacterWindow上でマウスが動くときに値を積算するNadenade添付プロパティの実装

        /// <summary>撫でた量を取得、設定する依存関係プロパティです。</summary>
        public static readonly DependencyProperty NadenadeProperty =
            DependencyProperty.Register("Nadenade", typeof(double), typeof(Window), new PropertyMetadata(0.0));

        /// <summary>撫でた量を取得、設定します。</summary>
        public double Nadenade
        {
            get { return (double)GetValue(NadenadeProperty); }
            set { SetValue(NadenadeProperty, value); }
        }

        /// <summary>添付プロパティから撫でた量を取得します。</summary>
        /// <param name="target">取得元</param>
        /// <returns>撫でた量</returns>
        public static double GetNadenade(Window target)
        {
            return (double)target.GetValue(NadenadeProperty);
        }
        /// <summary>添付プロパティに撫でた量を設定します。</summary>
        /// <param name="target">設定対象</param>
        /// <param name="value">撫でた量</param>
        public static void SetNadenade(Window target, double value)
        {
            target.SetValue(NadenadeProperty, value);
        }

        #endregion

        #endregion
    }
}
