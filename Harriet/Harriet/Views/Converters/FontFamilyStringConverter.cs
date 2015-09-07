using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Harriet.Views.Converters
{
    /// <summary>文字列とフォントファミリーとを変換します。</summary>
    public class FontFamilyStringConverter : IValueConverter
    {
        /// <summary>文字列をフォントファミリに変換します。</summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return new FontFamily((string)value);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// フォントファミリを文字列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FontFamily family = value as FontFamily;
            if (family != null && family.FamilyNames != null && family.FamilyNames.Values != null)
            {
                return family.FamilyNames.Values.First();
            }
            else
            {
                return "MS UI Gothic";
            }
        }
    }
}
