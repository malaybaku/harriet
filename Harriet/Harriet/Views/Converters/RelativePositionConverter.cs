using System;
using System.Windows;
using System.Windows.Data;

using Harriet.Models.Chat;

namespace Harriet.Views.Converters
{
    /// <summary>ラジオボタンでキャラ枠とチャット枠との相対位置を指定するためのコンバータです。</summary>
    public class RelativePositionConverter : IValueConverter
    {
        /// <summary>
        /// RelativePosition値とパラメタ文字列を用いてラジオボタンの状態を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(!((parameter is string) || (value is RelativePosition)))
            {
                return DependencyProperty.UnsetValue;
            }

            string pStr = (string)parameter;
            RelativePosition position = (RelativePosition)value;

            RelativePosition parameterPos = RelativePosition.None;
            string paramStr = parameter as string;
            if (paramStr != null)
            {
                Enum.TryParse(paramStr, out parameterPos);
            }

            return position == parameterPos;
        }

        /// <summary>
        /// ラジオボタンがtrueになったときRelativePositionとしてのパラメータを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //true以外の場合VMに飛んでいかない
            if (!(value is bool?) || (bool?)value != true)
            {
                return DependencyProperty.UnsetValue;
            }

            RelativePosition result = RelativePosition.None;
            string paramStr = parameter as string;
            if (paramStr != null)
            {
                Enum.TryParse(paramStr, out result);
            }
            return result;
        }
    }



}
