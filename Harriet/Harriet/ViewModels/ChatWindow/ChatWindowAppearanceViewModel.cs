using Livet.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Harriet.ViewModels
{
    /// <summary>チャット枠の表示/非表示を制御するビューモデルを表します。</summary>
    public class ChatWindowAppearanceViewModel : HarrietViewModelBase
    {
        public ChatWindowAppearanceViewModel(ChatWindowViewModel vm)
        {
            PropertyChangedEventManager.AddHandler(
                vm, 
                OnParentViewModelContentChanged,
                nameof(vm.Content)
                );   
        }

        #region プロパティ

        private Visibility _Visibility = Visibility.Hidden;
        /// <summary>ウィンドウが見えるかどうかを取得、設定します。</summary>
        public Visibility Visibility
        {
            get { return _Visibility; }
            set
            {
                if (_Visibility == value)
                    return;
                _Visibility = value;
                RaisePropertyChanged();
            }
        }

        //private Color _inactiveChromeColor = Colors.DarkGray;
        private Color _inactiveChromeColor = Colors.Transparent;
        /// <summary>チャット枠が非アクティブウィンドウのときの枠線の色を取得、設定します。</summary>
        public Color InactiveChromeColor
        {
            get { return _inactiveChromeColor; }
            set { SetAndRaisePropertyChanged(ref _inactiveChromeColor, value); }
        }

        private Color _activeChromeColor = Colors.DeepSkyBlue;
        /// <summary>チャット枠が非アクティブウィンドウのときの枠線の色を取得、設定します。</summary>
        public Color ActiveChromeColor
        {
            get { return _activeChromeColor; }
            set { SetAndRaisePropertyChanged(ref _activeChromeColor, value); }
        }

        #endregion

        #region HideCommand
        private ViewModelCommand _hideCommand;
        /// <summary>ウィンドウを隠すコマンドを取得します。</summary>
        public ViewModelCommand HideCommand
            => _hideCommand ?? (_hideCommand = new ViewModelCommand(Hide));

        /// <summary>ウィンドウを隠します。</summary>
        private void Hide()
        {
            InactiveChromeColor = Colors.Transparent;
            ActiveChromeColor = Colors.Transparent;
            Visibility = Visibility.Hidden;
        }
        #endregion

        /// <summary>ウィンドウが見えるようにします。</summary>
        public void Show()
        {
            InactiveChromeColor = Colors.DarkGray;
            ActiveChromeColor = Colors.DeepSkyBlue;
            Visibility = Visibility.Visible;
        }

        private void OnParentViewModelContentChanged(object sender, PropertyChangedEventArgs e)
        {
            Show();
        }

    }
}
