using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Livet.Commands;

using Harriet.Models.Chat;

namespace Harriet.ViewModels
{
    /// <summary>チャット枠のフォント設定を表します。</summary>
    public class ChatWindowTextViewModel : HarrietViewModelBase
    {
        public ChatWindowTextViewModel(IChatWindowTextSetting setting)
        {
            FontSize = setting.FontSize;
            FontFamilyName = setting.FontFamilyName;
            IsFontBold = setting.IsFontBold;
            IsFontItalic = setting.IsFontItalic;

            AssignToSetting(setting);
        }

        private const string DefaultFontFamilyName = "MS UI Gothic";

        #region FontSize変更通知プロパティ
        private double _fontSize = 12.0;
        /// <summary>フォントの大きさを取得、設定します。</summary>
        public double FontSize
        {
            get { return _fontSize; }
            set { SetAndRaisePropertyChanged(ref _fontSize, value); }
        }
        #endregion

        #region FontFamilyName変更通知プロパティ
        private string _fontFamilyName = DefaultFontFamilyName;
        /// <summary>フォントファミリーの名前を取得、設定します。</summary>
        public string FontFamilyName
        {
            get { return _fontFamilyName; }
            set { SetAndRaisePropertyChanged(ref _fontFamilyName, value); }
        }
        #endregion

        #region IsFontBold変更通知プロパティ
        private bool _isFontBold;
        /// <summary>フォントを太字にするかを</summary>
        public bool IsFontBold
        {
            get { return _isFontBold; }
            set { SetAndRaisePropertyChanged(ref _isFontBold, value); }
        }
        #endregion

        #region IsFontItalic変更通知プロパティ
        private bool _isFontItalic;
        /// <summary>フォントをイタリック体にするかを取得、設定します。</summary>
        public bool IsFontItalic
        {
            get { return _isFontItalic; }
            set { SetAndRaisePropertyChanged(ref _isFontItalic, value); }
        }
        #endregion

        #region SelectFontCommand
        private ViewModelCommand _selectFontCommand;
        /// <summary>フォント選択を行うコマンドを取得します。</summary>
        public ViewModelCommand SelectFontCommand
            => _selectFontCommand ?? (_selectFontCommand = new ViewModelCommand(SelectFont));

        #endregion

        /// <summary>
        /// フォントを選択します。
        /// TODO: 透明度が選択できるようにExtended WPF Toolkitとか使って改善すべき
        /// </summary>
        public void SelectFont()
        {
            var dialog = new FontDialog();
            dialog.Font = new Font(
                FontFamilyName,
                (float)FontSize,
                (IsFontBold ? FontStyle.Bold : FontStyle.Regular) |
                    (IsFontItalic ? FontStyle.Italic : FontStyle.Regular)
                );

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FontFamilyName = dialog.Font.Name;
            FontSize = dialog.Font.SizeInPoints;
            IsFontBold = dialog.Font.Bold;
            IsFontItalic = dialog.Font.Italic;
        }

        private void AssignToSetting(IChatWindowTextSetting setting)
        {
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(FontSize))
                {
                    setting.FontSize = FontSize;
                }
                else if (e.PropertyName == nameof(FontFamilyName))
                {
                    setting.FontFamilyName = FontFamilyName;
                }
                else if (e.PropertyName == nameof(IsFontBold))
                {
                    setting.IsFontBold = IsFontBold;
                }
                else if (e.PropertyName == nameof(IsFontItalic))
                {
                    setting.IsFontItalic = IsFontItalic;
                }
            };

            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);

        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is IChatWindowTextSetting)) return;

            var setting = sender as IChatWindowTextSetting;
            if (e.PropertyName == nameof(FontSize))
            {
                FontSize = setting.FontSize;
            }
            else if (e.PropertyName == nameof(FontFamilyName))
            {
                FontFamilyName = setting.FontFamilyName;
            }
            else if (e.PropertyName == nameof(IsFontBold))
            {
                IsFontBold = setting.IsFontBold;
            }
            else if (e.PropertyName == nameof(IsFontItalic))
            {
                IsFontItalic = setting.IsFontItalic;
            }
        }
    }
}
