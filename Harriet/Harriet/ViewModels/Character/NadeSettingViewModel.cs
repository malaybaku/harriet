using System.ComponentModel;

using Harriet.Models;

namespace Harriet.ViewModels
{
    /// <summary>撫でる操作に関する設定を表します。</summary>
    public class NadeSettingViewModel : HarrietViewModelBase
    {
        public NadeSettingViewModel(INadeSetting setting)
        {
            DecreasePerFrame = setting.DecreasePerFrame;
            Max = setting.Max;

            AssignToSetting(setting);
        }

        private double _DecreasePerFrame = 5.0;
        /// <summary>1フレーム当たりの撫で減少値を取得、設定します。</summary>
        public double DecreasePerFrame
        {
            get { return _DecreasePerFrame; }
            set { SetAndRaisePropertyChanged(ref _DecreasePerFrame, value); }
        }

        private double _Max = 5000.0;
        /// <summary>撫で値の最大値を取得、設定します。</summary>
        public double Max
        {
            get { return _Max; }
            set { SetAndRaisePropertyChanged(ref _Max, value); }
        }

        private void AssignToSetting(INadeSetting setting)
        {
            PropertyChanged += (_, e) =>
            {
                if(e.PropertyName == nameof(DecreasePerFrame))
                {
                    setting.DecreasePerFrame = DecreasePerFrame;
                }
                else if(e.PropertyName == nameof(Max))
                {
                    setting.Max = Max;
                }
            };
            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is INadeSetting)) return;

            var setting = sender as INadeSetting;

            if(e.PropertyName == nameof(DecreasePerFrame))
            {
                DecreasePerFrame = setting.DecreasePerFrame;
            }
            else if(e.PropertyName == nameof(Max))
            {
                Max = setting.Max;
            }
        }
    }
}
