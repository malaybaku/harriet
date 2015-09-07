using System.ComponentModel;

namespace Harriet.Models.Voice
{
    /// <summary>音声の設定内容を定義します。</summary>
    public interface IVoiceSetting : INotifyPropertyChanged
    {
        /// <summary>声の種類を取得、設定します。</summary>
        string VoiceType { get; set; }
        /// <summary>音量を取得、設定します。100を基準値とし、0は無音を表します。</summary>
        int Volume { get; set; }
        /// <summary>音声の再生速度を取得、設定します。100を基準値とします。</summary>
        int Speed { get; set; }
        /// <summary>音声の高さを取得、設定します。100を基準値とします。</summary>
        int Pitch { get; set; }
    }

    /// <summary>音声の設定内容を定義します。</summary>
    public class VoiceSetting : HarrietNotifiableModelBase, IVoiceSetting
    {
        /// <summary>既定値でインスタンスを初期化します。</summary>
        public VoiceSetting()
        {
            VoiceType = "女性1";
            Volume = 50;
            Speed = 100;
            Pitch = 100;
        }

        private string _voiceType = "女性1";
        /// <summary>声の種類を取得、設定します。</summary>
        public string VoiceType
        {
            get { return _voiceType; }
            set { SetAndRaisePropertyChanged(ref _voiceType, value); }
        }

        private const int VolumeMin = 0;
        private const int VolumeMax = 300;
        private int _volume = 50;
        /// <summary>音量を取得、設定します。100を基準値とし、0は無音を表します。</summary>
        public int Volume
        {
            get { return _volume; }
            set
            {
                if(_volume != value && value >= VolumeMin && value <= VolumeMax)
                {
                    _volume = value;
                    RaisePropertyChanged();
                }
            }
        }

        private const int SpeedMin = 50;
        private const int SpeedMax = 200;
        private int _speed = 100;
        /// <summary>音声の再生速度を取得、設定します。100を基準値とします。</summary>
        public int Speed
        {
            get { return _speed; }
            set
            {
                if(_speed != value && value >= SpeedMin && value <= SpeedMax)
                {
                    _speed = value;
                    RaisePropertyChanged();
                }
            }
        }

        private const int PitchMin = 50;
        private const int PitchMax = 200;
        private int _pitch = 100;
        /// <summary>音声の高さを取得、設定します。100を基準値とします。</summary>
        public int Pitch
        {
            get { return _pitch; }
            set
            {
                if(_pitch != value && value >= PitchMin && value <= PitchMax)
                {
                    _pitch = value;
                    RaisePropertyChanged();
                }
            }
        }
    }

}
