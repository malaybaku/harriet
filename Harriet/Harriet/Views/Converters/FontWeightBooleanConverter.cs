using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Harriet.Views.Converters
{
    /// <summary>フォントの太さをブール値と変換するコンバータです。</summary>
    public class FontWeightBooleanConverter: IValueConverter
    {
        /// <summary>
        /// ブール値からフォントの太さを取得します。
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
                return (bool)value ? FontWeights.Bold : FontWeights.Normal;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// フォントの太さからブール値を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is FontWeight)
            {
                FontWeight weight = (FontWeight)value;
                return weight == FontWeights.Bold;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
