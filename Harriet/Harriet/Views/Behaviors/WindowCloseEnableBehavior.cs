using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

//パク…参考元: http://blog.xin9le.net/entry/2013/11/06/021557
namespace Harriet.Views.Behaviors
{
    /// <summary>システムメニュー関連のビヘイビアを定義します。</summary>
    public class SystemMenuBehavior : Behavior<Window>
    {
        [DllImport("user32.dll")]
        private extern static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        #region Properties

        /// <summary>表示/非表示を取得、設定します。</summary>
        public bool? IsVisible
        {
            get { return (bool?)this.GetValue(IsVisibleProperty); }
            set { this.SetValue(IsVisibleProperty, value); }
        }
        /// <summary>表示/非表示の設定を依存関係プロパティとして定義します。</summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool?), typeof(SystemMenuBehavior), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>最小化の有効性を取得、設定します。</summary>
        public bool? CanMinimize
        {
            get { return (bool?)this.GetValue(CanMinimizeProperty); }
            set { this.SetValue(CanMinimizeProperty, value); }
        }
        /// <summary>最小化可能の設定を依存関係プロパティとして定義します。</summary>
        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool?), typeof(SystemMenuBehavior), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>最大化の有効性を取得、設定します。</summary>
        public bool? CanMaximize
        {
            get { return (bool?)this.GetValue(CanMaximizeProperty); }
            set { this.SetValue(CanMaximizeProperty, value); }
        }
        /// <summary>最大化可能の設定を依存関係プロパティとして定義します。</summary>
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool?), typeof(SystemMenuBehavior), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>ヘルプ表示の有効性を取得、設定します。</summary>
        public bool? ShowContextHelp
        {
            get { return (bool?)this.GetValue(ShowContextHelpProperty); }
            set { this.SetValue(ShowContextHelpProperty, value); }
        }
        /// <summary>ヘルプ表示の設定を依存関係プロパティとして定義します。</summary>
        public static readonly DependencyProperty ShowContextHelpProperty = DependencyProperty.Register("ShowContextHelp", typeof(bool?), typeof(SystemMenuBehavior), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>Alt+F4操作の有効性を取得、設定します。</summary>
        public bool EnableAltF4
        {
            get { return (bool)this.GetValue(EnableAltF4Property); }
            set { this.SetValue(EnableAltF4Property, value); }
        }
        /// <summary>Alt+F4操作の有効性を依存関係プロパティとして定義します。</summary>
        public static readonly DependencyProperty EnableAltF4Property = DependencyProperty.Register("EnableAltF4", typeof(bool), typeof(SystemMenuBehavior), new PropertyMetadata(true));
        #endregion

        /// <summary>ヘルプボタンを押すと発生します。</summary>
        public event EventHandler ContextHelpClick = null;

        /// <summary>ビヘイビアの開始処理です。</summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.SourceInitialized += this.OnSourceInitialized;
            base.OnAttached();
        }

        /// <summary>ビヘイビアの終了処理です。</summary>
        protected override void OnDetaching()
        {
            var source = (HwndSource)HwndSource.FromVisual(this.AssociatedObject);
            source.RemoveHook(this.HookProcedure);
            this.AssociatedObject.SourceInitialized -= this.OnSourceInitialized;
            base.OnDetaching();
        }


        #region Event Handlers
        //--- プロパティが変更されたら表示更新
        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var self = obj as SystemMenuBehavior;
            if (self != null)
                self.Apply();
        }

        //--- Windowハンドルを取得できるようになったタイミングで初期化
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            this.Apply();
            var source = (HwndSource)HwndSource.FromVisual(this.AssociatedObject);
            source.AddHook(this.HookProcedure);  //--- メッセージフック
        }

        //--- メッセージ処理
        private IntPtr HookProcedure(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //--- コンテキストヘルプをクリックした
            if (msg == Constant.WM_SYSCOMMAND)
                if (wParam.ToInt32() == Constant.SC_CONTEXTHELP)
                {
                    handled = true;
                    var handler = this.ContextHelpClick;
                    if (handler != null)
                        handler(this.AssociatedObject, EventArgs.Empty);
                }

            //--- Alt + F4を無効化
            if (!this.EnableAltF4)
                if (msg == Constant.WM_SYSKEYDOWN)
                    if (wParam.ToInt32() == Constant.VK_F4)
                        handled = true;

            //--- ok
            return IntPtr.Zero;
        }
        #endregion

        #region Methods
        //--- 設定を反映
        private void Apply()
        {
            if (this.AssociatedObject == null)
                return;

            //--- スタイル
            var hwnd = new WindowInteropHelper(this.AssociatedObject).Handle;
            var style = GetWindowLong(hwnd, Constant.GWL_STYLE);
            if (this.IsVisible.HasValue)
            {
                if (this.IsVisible.Value) style |= Constant.WS_SYSMENU;
                else style &= ~Constant.WS_SYSMENU;
            }
            if (this.CanMinimize.HasValue)
            {
                if (this.CanMinimize.Value) style |= Constant.WS_MINIMIZEBOX;
                else style &= ~Constant.WS_MINIMIZEBOX;
            }
            if (this.CanMaximize.HasValue)
            {
                if (this.CanMaximize.Value) style |= Constant.WS_MAXIMIZEBOX;
                else style &= ~Constant.WS_MAXIMIZEBOX;
            }
            SetWindowLong(hwnd, Constant.GWL_STYLE, style);

            //--- 拡張スタイル
            var exStyle = GetWindowLong(hwnd, Constant.GWL_EXSTYLE);
            if (this.ShowContextHelp.HasValue)
            {
                if (this.ShowContextHelp.Value) exStyle |= Constant.WS_EX_CONTEXTHELP;
                else exStyle &= ~Constant.WS_EX_CONTEXTHELP;
            }
            SetWindowLong(hwnd, Constant.GWL_EXSTYLE, exStyle);
        }
        #endregion

        class Constant
        {
            //--- GetWindowLong
            public const int GWL_STYLE = -16;
            public const int GWL_EXSTYLE = -20;
            //--- Window Style
            public const int WS_EX_CONTEXTHELP = 0x00400;
            public const int WS_MAXIMIZEBOX = 0x10000;
            public const int WS_MINIMIZEBOX = 0x20000;
            public const int WS_SYSMENU = 0x80000;
            //--- Window Message
            public const int WM_SYSKEYDOWN = 0x0104;
            public const int WM_SYSCOMMAND = 0x0112;
            //--- System Command
            public const int SC_CONTEXTHELP = 0xF180;
            //--- Virtual Keyboard
            public const int VK_F4 = 0x73;
            //--- Constructor
            private Constant() { }
        }
    }
}
