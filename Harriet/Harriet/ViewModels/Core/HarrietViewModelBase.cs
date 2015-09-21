using System;
using System.Runtime.CompilerServices;

using Livet;

namespace Harriet.ViewModels
{
    /// <summary>LivetのViewModelにユーティリティ関数を追加したビューモデル実装を表します。</summary>
    public class HarrietViewModelBase : ViewModel
    {
        /// <summary>文字列を代入しつつ必要ならPropertyChangedイベントを発生させます。</summary>
        /// <param name="target">代入対象となるフィールド</param>
        /// <param name="value">代入したい値</param>
        /// <param name="propertyName">プロパティ名</param>
        protected void SetAndRaisePropertyChanged(ref string target, string value, [CallerMemberName]string propertyName="")
        {
            if(target != value)
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
        protected void SetAndRaisePropertyChanged<T>(ref T target, T value, [CallerMemberName]string propertyName="")
            where T : struct, IEquatable<T>
        {
            if (!target.Equals(value))
            {
                target = value;
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>文字列を代入しつつ必要ならUIスレッド上でPropertyChangedイベントを発生させます。</summary>
        /// <param name="target">代入対象となるフィールド</param>
        /// <param name="value">代入したい値</param>
        /// <param name="propertyName">プロパティ名</param>
        protected void UISetAndRaisePropertyChanged(ref string target, string value, [CallerMemberName]string propertyName="")
        {
            if (target == value) return;

            target = value;
            DispatcherHelper.UIDispatcher.Invoke(() => RaisePropertyChanged(propertyName));
        }

        /// <summary>structのプロパティに値をセットし、必要ならUIスレッド上でPropertyChangedイベントを発生させます。</summary>
        /// <typeparam name="T">プロパティ(のバッキングフィールド)の型</typeparam>
        /// <param name="target">代入先のフィールド</param>
        /// <param name="value">代入したい値</param>
        /// <param name="propertyName">プロパティ名</param>
        protected void UISetAndRaisePropertyChanged<T>(ref T target, T value, [CallerMemberName]string propertyName = "")
            where T : struct, IEquatable<T>
        {
            if (target.Equals(value)) return;

            target = value;
            DispatcherHelper.UIDispatcher.Invoke(() => RaisePropertyChanged(propertyName));
        }

    }
}
