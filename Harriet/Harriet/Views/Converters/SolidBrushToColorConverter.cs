using System;

using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Harriet.Views
{
    /// <summary>SolidColorBrushとColorを相互に変換します。</summary>
    public class SolidBrushToColorConverter : IValueConverter
    {
        /// <summary>
        /// Colorを用いてSolidColorBrushを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is Color)
            {
                return new SolidColorBrush((Color)value);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// SolidColorBrushで使用しているColorを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                return (value as SolidColorBrush).Color;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
