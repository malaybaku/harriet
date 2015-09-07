using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Harriet.Models.Chat
{
    /// <summary>チャット枠の色設定を定義します。</summary>
    public interface IChatWindowColorSetting : INotifyPropertyChanged
    {
        /// <summary>背景色を色として取得、設定します。</summary>
        Color BackgroundColor { get; set; }

        /// <summary>前面色(文字色)を色として取得、設定します。</summary>
        Color ForegroundColor { get; set; }
    }

    /// <summary>チャット窓の色設定を表します。</summary>
    public class ChatWindowColorSetting : HarrietNotifiableModelBase, IChatWindowColorSetting
    {

        private Color _background = Colors.White;
        /// <summary>背景色を色として取得、設定します。</summary>
        [DataMember]
        public Color BackgroundColor
        {
            get { return _background; }
            set { SetAndRaisePropertyChanged(ref _background, value); }
        }

        private Color _foreground = Colors.Black;
        /// <summary>前面色(文字色)を色として取得、設定します。</summary>
        [DataMember]
        public Color ForegroundColor
        {
            get { return _foreground; }
            set { SetAndRaisePropertyChanged(ref _foreground, value); }
        }

    }
}
