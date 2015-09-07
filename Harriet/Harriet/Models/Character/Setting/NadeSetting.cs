using System.ComponentModel;

namespace Harriet.Models
{
    /// <summary>撫でる操作に関する設定を定義します。</summary>
    public interface INadeSetting : INotifyPropertyChanged
    {
        /// <summary>1フレーム当たりの撫で減少値を取得、設定します。</summary>
        double DecreasePerFrame { get; set; }

        /// <summary>撫で値の最大値を取得、設定します。</summary>
        double Max { get; set; }

    }

    /// <summary>撫でる操作に関する設定を定義します。</summary>
    public class NadeSetting : HarrietNotifiableModelBase, INadeSetting
    {
        private double _decreasePerFrame = 5;
        /// <summary>1フレーム当たりの撫で減少値を取得、設定します。</summary>
        public double DecreasePerFrame
        {
            get { return _decreasePerFrame; }
            set { SetAndRaisePropertyChanged(ref _decreasePerFrame, value); }
        }

        private double _max = 5000;
        /// <summary>撫で値の最大値を取得、設定します。</summary>
        public double Max
        {
            get { return _max; }
            set { SetAndRaisePropertyChanged(ref _max, value); }
        }
    }
}
