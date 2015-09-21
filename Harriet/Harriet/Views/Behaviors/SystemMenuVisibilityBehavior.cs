using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Interactivity;

//パク…参考元: http://grabacr.net/archives/208
namespace Harriet.Views.Behaviors
{
    /// <summary>システムメニューを隠すビヘイビアを定義します。</summary>
    public class HideSystemMenuBehavior : Behavior<Window>
    {
        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x00080000;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int VK_F4 = 0x73;

        /// <summary>システムメニューを表示しないように設定します。</summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SourceInitialized += (sender, e) =>
            {
                var handle = new WindowInteropHelper(this.AssociatedObject).Handle;

                var windowStyle = GetWindowLong(handle, GWL_STYLE);
                windowStyle &= ~WS_SYSMENU;
                SetWindowLong(handle, GWL_STYLE, windowStyle);

                var hwndSource = HwndSource.FromHwnd(handle);
                hwndSource.AddHook(this.WndProc);
            };
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SYSKEYDOWN && wParam.ToInt32() == VK_F4)
            {
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
