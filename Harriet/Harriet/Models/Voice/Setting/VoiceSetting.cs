using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Harriet.Models.Voice
{
    /// <summary>音声の設定内容を定義します。</summary>
    public interface IVoiceSetting : INotifyPropertyChanged
    {
        /// <summary>声の種類を取得、設定します。</summary>
        string VoiceType { get; set; }
        /// <summary>平文/発音記号変換器の種類を取得、設定します。</summary>
        string TextConverterType { get; set; }

        /// <summary>音量を取得、設定します。100を基準値とし、0は無音を表します。</summary>
        int Volume { get; set; }
        /// <summary>音声の再生速度を取得、設定します。100を基準値とします。</summary>
        int Speed { get; set; }
        /// <summary>音声の高さを取得、設定します。100を基準値とします。</summary>
        int Pitch { get; set; }

        /// <summary>利用可能な声種の一覧を取得します。</summary>
        IReadOnlyList<string> AvailableVoices { get; }

        /// <summary>利用可能な平文/発音変換器の一覧を取得します。</summary>
        IReadOnlyList<string> AvailableTextConverters { get; }
    }

    /// <summary>音声の設定内容を定義します。</summary>
    public class VoiceSetting : HarrietNotifiableModelBase, IVoiceSetting
    {
        //とくにコンストラクタでは何もしない

        private string _voiceType = AquesVoiceConstNames.NameF1;
        /// <summary>声の種類を取得、設定します。</summary>
        public string VoiceType
        {
            get { return _voiceType; }
            set
            {
                //不正入力(存在しない音声合成器)をガード
                if(AvailableVoices.Contains(value))
                {
                    SetAndRaisePropertyChanged(ref _voiceType, value);
                }
            }
        }

        private string _textConverterType = EmptyTextToPronounceConverter.ConverterName;
        /// <summary>声の種類を取得、設定します。</summary>
        public string TextConverterType
        {
            get { return _textConverterType; }
            set
            {
                if(AvailableTextConverters.Contains(value))
                {
                    SetAndRaisePropertyChanged(ref _textConverterType, value);
                }
            }
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



        private IReadOnlyList<string> _availableVoices;
        /// <summary>利用可能な声種の一覧を取得、設定します。</summary>
        [IgnoreDataMember]
        public IReadOnlyList<string> AvailableVoices
        {
            get
            {
                return _availableVoices ?? (_availableVoices = VoiceSynthesizerLoader.LoadAvailableVoices()
                .Select(v => v.Name)
                .ToList()
                .AsReadOnly());
            }
        }

        private IReadOnlyList<string> _availableTextConverters;
        /// <summary>利用可能な平文/発音変換器の一覧を取得します。</summary>
        [IgnoreDataMember]
        public IReadOnlyList<string> AvailableTextConverters
        {
            get
            {
                return _availableTextConverters ?? (_availableTextConverters =
                                TextToPronounceConverterLoader.LoadAvailableTextConverters()
                                .Keys
                                .ToList()
                                .AsReadOnly());
            }
        }
    }

}
