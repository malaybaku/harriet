using System.Windows.Media;

using Harriet.Models.Chat;
using System.ComponentModel;

namespace Harriet.ViewModels
{
    /// <summary>チャット窓の色設定のビューモデルを表します。</summary>
    public class ChatWindowColorViewModel : HarrietViewModelBase
    {
        public ChatWindowColorViewModel(IChatWindowColorSetting setting)
        {
            Foreground = new SolidColorBrush(setting.ForegroundColor);
            Background = new SolidColorBrush(setting.BackgroundColor);

            AssignToSetting(setting);
        }

        //TODO: 色が変わった時にBrushごと変更するのは若干ムダ遣いな感じがあるので
        //Colorに直してほしい

        #region Background変更通知プロパティ
        private SolidColorBrush _Background = Brushes.White;
        /// <summary>背景色を取得、設定します。</summary>
        public SolidColorBrush Background
        {
            get { return _Background; }
            set
            {
                if (_Background.Color != value.Color)
                {
                    _Background = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Foreground変更通知プロパティ
        private SolidColorBrush _Foreground = Brushes.Black;
        /// <summary>チャット枠の前面色(文字色)を取得、設定します。</summary>
        public SolidColorBrush Foreground
        {
            get { return _Foreground; }
            set
            { 
                if (_Foreground.Color != value.Color)
                {
                    _Foreground = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>プロパティ変更をモデルに流す</summary>
        /// <param name="setting">設定内容を持ってるモデル</param>
        private void AssignToSetting(IChatWindowColorSetting setting)
        {
            PropertyChanged += (_, e) =>
            {
                if(e.PropertyName == nameof(Foreground))
                {
                    setting.ForegroundColor = Foreground.Color;
                }
                else if(e.PropertyName == nameof(Background))
                {
                    setting.BackgroundColor = Background.Color;
                }
            };
            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is IChatWindowColorSetting)) return;

            var setting = sender as IChatWindowColorSetting;

            if (e.PropertyName == nameof(setting.ForegroundColor))
            {
                Foreground = new SolidColorBrush(setting.ForegroundColor);
            }
            else if(e.PropertyName == nameof(setting.BackgroundColor))
            {
                Background = new SolidColorBrush(setting.BackgroundColor);
            }

        }

    }
}
