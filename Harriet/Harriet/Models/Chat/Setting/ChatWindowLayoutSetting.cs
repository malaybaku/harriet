
using System.ComponentModel;

namespace Harriet.Models.Chat
{
    /// <summary>チャット窓の表示位置設定を定義します。</summary>
    public interface IChatWindowLayoutSetting : INotifyPropertyChanged
    {
        /// <summary>幅の既定値を取得、設定します。</summary>
        double DefaultWidth { get; set; }

        /// <summary>高さの既定値を取得、設定します。</summary>
        double DefaultHeight { get; set; }

        /// <summary>キャラに対するチャット枠の相対位置を取得、設定します。</summary>
        RelativePosition RelativePosition { get; set; }
    }

    public class ChatWindowLayoutSetting : HarrietNotifiableModelBase, IChatWindowLayoutSetting
    {
        private double _defaultWidth = 300.0;
        /// <summary>幅の既定値を取得、設定します。</summary>
        public double DefaultWidth
        {
            get { return _defaultWidth; }
            set { SetAndRaisePropertyChanged(ref _defaultWidth, value); }
        }

        private double _defaultHeight = 150.0;
        /// <summary>高さの既定値を取得、設定します。</summary>
        public double DefaultHeight
        {
            get { return _defaultHeight; }
            set { SetAndRaisePropertyChanged(ref _defaultHeight, value); }
        }

        private RelativePosition _relativePosition = RelativePosition.LeftTop;
        /// <summary>キャラに対するチャット枠の相対位置を取得、設定します。</summary>
        public RelativePosition RelativePosition
        {
            get { return _relativePosition; }
            set
            {
                if(_relativePosition != value)
                {
                    _relativePosition = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
