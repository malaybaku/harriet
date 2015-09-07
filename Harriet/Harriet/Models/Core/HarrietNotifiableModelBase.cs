using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Harriet.Models
{
    /// <summary>モデルへINotifyPropertyChangedの簡単な実装を提供します。</summary>
    public class HarrietNotifiableModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>文字列を代入しつつ必要ならPropertyChangedイベントを発生させます。</summary>
        /// <param name="target">代入対象となるフィールド</param>
        /// <param name="value">代入したい値</param>
        /// <param name="propertyName">プロパティ名</param>
        protected void SetAndRaisePropertyChanged(ref string target, string value, [CallerMemberName]string propertyName = "")
        {
            //NOTE: ぬるりエラーは許されない
            if (value != null && target != value)
            {
                target = value;
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>structのプロパティに値をセットし、必要ならPropertyChangedイベントを発生させます。</summary>
        /// <typeparam name="T">プロパティ(のバッキングフィールド)の型</typeparam>
        /// <param name="target">代入先のフィールド</param>
        /// <param name="value">代入したい値</param>
        /// <param name="propertyName">プロパティ名</param>
        protected void SetAndRaisePropertyChanged<T>(ref T target, T value, [CallerMemberName]string propertyName = "")
            where T : struct, IEquatable<T>
        {
            if (!target.Equals(value))
            {
                target = value;
                RaisePropertyChanged(propertyName);
            }
        }

    }
}
