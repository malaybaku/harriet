using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Harriet.Views.Converters
{
    /// <summary>文字列を明示的にTextBlockに変換します。</summary>
    class StringToTextBlockConverterIValueConverter : IValueConverter
    {
        /// <summary>
        /// 文字列は明示的にTextBlockに変換し、それ以外は素通し。
        /// NOTE: 明示変換が必要なのは親要素で定義されているスタイルを引き継ぐため
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                return new TextBlock() { Text = (string)value };
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 逆変換をサポートしません。
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
