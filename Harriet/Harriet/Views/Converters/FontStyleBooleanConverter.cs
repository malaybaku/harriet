using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Harriet.Views.Converters
{
    /// <summary>フォント表示(斜体かどうか)をブール値と変換するコンバータです。</summary>
    public class FontStyleBooleanConverter : IValueConverter
    {
        /// <summary>
        /// ブール値からフォントのスタイルを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            {
                return (bool)value ? FontStyles.Italic : FontStyles.Normal;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// フォントスタイルからブール値を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is FontStyle)
            {
                FontStyle style = (FontStyle)value;
                return style != FontStyles.Normal;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
