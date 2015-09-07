using System.ComponentModel;

namespace Harriet.Models.Chat
{
    /// <summary>チャット枠のフォント設定を定義します。</summary>
    public interface IChatWindowTextSetting : INotifyPropertyChanged
    {
        /// <summary>フォントの大きさを取得、設定します。</summary>
        double FontSize { get; set; }

        /// <summary>フォントファミリーの名前を取得、設定します。</summary>
        string FontFamilyName { get; set; }

        /// <summary>フォントを太字にするかを</summary>
        bool IsFontBold { get; set; }

        /// <summary>フォントをイタリック体にするかを取得、設定します。</summary>
        bool IsFontItalic { get; set; }
    }

    /// <summary>チャット枠のフォント設定を表します。</summary>
    public class ChatWindowTextSetting : HarrietNotifiableModelBase, IChatWindowTextSetting
    {
        private const string DefaultFontFamilyName = "MS UI Gothic";

        private double _fontSize = 11.0;
        /// <summary>フォントの大きさを取得、設定します。</summary>
        public double FontSize
        {
            get { return _fontSize; }
            set { SetAndRaisePropertyChanged(ref _fontSize, value); }
        }

        private string _fontFamily = DefaultFontFamilyName;
        /// <summary>フォントファミリーの名前を取得、設定します。</summary>
        public string FontFamilyName
        {
            get { return _fontFamily; }
            set { SetAndRaisePropertyChanged(ref _fontFamily, value); }
        }

        private bool _isFontBold;
        /// <summary>フォントを太字にするかを</summary>
        public bool IsFontBold
        {
            get { return _isFontBold; }
            set { SetAndRaisePropertyChanged(ref _isFontBold, value); }
        }

        private bool _isFontItalic;
        /// <summary>フォントをイタリック体にするかを取得、設定します。</summary>
        public bool IsFontItalic
        {
            get { return _isFontItalic; }
            set { SetAndRaisePropertyChanged(ref _isFontItalic, value); }
        }

    }

}
