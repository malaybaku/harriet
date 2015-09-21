using Harriet.Models.Chat;
using System.ComponentModel;

namespace Harriet.ViewModels
{
    /// <summary>チャット枠の配置設定を表します。</summary>
    public class ChatWindowLayoutViewModel : HarrietViewModelBase
    {
        public ChatWindowLayoutViewModel(IChatWindowLayoutSetting setting)
        {
            DefaultWidth = setting.DefaultWidth;
            DefaultHeight = setting.DefaultHeight;
            RelativePosition = setting.RelativePosition;

            AssignToSetting(setting);
        }

        #region DefaultWidth変更通知プロパティ
        private double _DefaultWidth = 150;
        /// <summary>幅の既定値を取得、設定します。</summary>
        public double DefaultWidth
        {
            get { return _DefaultWidth; }
            set { SetAndRaisePropertyChanged(ref _DefaultWidth, value); }
        }
        #endregion

        #region DefaultHeight変更通知プロパティ
        private double _DefaultHeight = 300;
        /// <summary>高さの既定値を取得、設定します。</summary>
        public double DefaultHeight
        {
            get { return _DefaultHeight; }
            set { SetAndRaisePropertyChanged(ref _DefaultHeight, value); }
        }
        #endregion

        #region RelativePosition変更通知プロパティ
        private RelativePosition _RelativePosition;
        /// <summary>キャラに対するチャット枠の相対位置を取得、設定します。</summary>
        public RelativePosition RelativePosition
        {
            get { return _RelativePosition; }
            set
            {
                if(_RelativePosition != value)
                {
                    _RelativePosition = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        private void AssignToSetting(IChatWindowLayoutSetting setting)
        {
            PropertyChanged += (_, e) =>
            {
                if(e.PropertyName == nameof(DefaultWidth))
                {
                    setting.DefaultWidth = this.DefaultWidth;
                }
                else if(e.PropertyName == nameof(DefaultHeight))
                {
                    setting.DefaultHeight = this.DefaultHeight;
                }
                else if(e.PropertyName == nameof(RelativePosition))
                {
                    setting.RelativePosition = this.RelativePosition;
                }
            };
            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is IChatWindowLayoutSetting)) return;

            var setting = sender as IChatWindowLayoutSetting;
            if (e.PropertyName == nameof(DefaultWidth))
            {
                DefaultWidth = setting.DefaultWidth;
            }
            else if (e.PropertyName == nameof(DefaultHeight))
            {
                DefaultHeight = setting.DefaultHeight;
            }
            else if (e.PropertyName == nameof(RelativePosition))
            {
                RelativePosition = setting.RelativePosition;
            }

        }
    }
}
