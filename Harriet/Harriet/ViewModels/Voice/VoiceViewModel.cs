using System.ComponentModel;
using Harriet.Models.Voice;

namespace Harriet.ViewModels
{
    /// <summary>音声の設定を表します。</summary>
    public class VoiceViewModel : HarrietViewModelBase
    {
        public VoiceViewModel(IVoiceSetting setting)
        {
            VoiceType = setting.VoiceType;
            Volume = setting.Volume;
            Speed = setting.Speed;
            Pitch = setting.Pitch;

            AssignToSetting(setting);
        }

        /// <summary>選択可能なすべての声種を取得します。</summary>
        public string[] AllVoices
        {
            get
            {
                return new string[]{
                    "女性1",
                    "女性2",
                    "男性1",
                    "男性2",
                    "ロボット",
                    "中性",
                    "機械",
                    "特殊"
                };
            }
        }

        private string _VoiceType = "女性1";
        /// <summary>声の種類を取得、設定します。</summary>
        public string VoiceType
        {
            get { return _VoiceType; }
            set { SetAndRaisePropertyChanged(ref _VoiceType, value); }
        }

        private int _Volume = 50;
        /// <summary>音量を取得、設定します。</summary>
        public int Volume
        {
            get { return _Volume; }
            set { SetAndRaisePropertyChanged(ref _Volume, value); }
        }

        private int _Speed = 100;
        /// <summary>喋る速度を取得、設定します。</summary>
        public int Speed
        {
            get { return _Speed; }
            set { SetAndRaisePropertyChanged(ref _Speed, value); }
        }

        private int _Pitch = 100;
        /// <summary>喋りの声の高さを取得、設定します。</summary>
        public int Pitch
        {
            get { return _Pitch; }
            set { SetAndRaisePropertyChanged(ref _Pitch, value); }
        }

        private void AssignToSetting(IVoiceSetting setting)
        {
            PropertyChanged += (_, e) =>
            {
                if(e.PropertyName == nameof(VoiceType))
                {
                    setting.VoiceType = VoiceType;
                }
                else if(e.PropertyName == nameof(Volume))
                {
                    setting.Volume = Volume;
                }
                else if(e.PropertyName == nameof(Speed))
                {
                    setting.Speed = Speed;
                }
                else if(e.PropertyName == nameof(Pitch))
                {
                    setting.Pitch = Pitch;
                }
            };
            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is IVoiceSetting)) return;

            var setting = sender as IVoiceSetting;

            if (e.PropertyName == nameof(VoiceType))
            {
                VoiceType = setting.VoiceType;
            }
            else if (e.PropertyName == nameof(Volume))
            {
                Volume = setting.Volume;
            }
            else if (e.PropertyName == nameof(Speed))
            {
                Speed = setting.Speed;
            }
            else if (e.PropertyName == nameof(Pitch))
            {
                Pitch = setting.Pitch;
            }

        }

    }
}
