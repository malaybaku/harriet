using System.ComponentModel;

namespace Harriet.Models
{
    /// <summary>キャラクターの表示設定内容を定義します。</summary>
    public interface ICharacterAppearanceSetting : INotifyPropertyChanged
    {
        /// <summary>キャラクターのサイズ倍率を取得、設定します。</summary>
        double SizeScale { get; set; }
    }

    /// <summary>キャラの表示設定を表します。</summary>
    public class CharacterAppearanceSetting : HarrietNotifiableModelBase, ICharacterAppearanceSetting
    {

        private double _sizeScale = 1.0;
        /// <summary>キャラクターのサイズ倍率を取得、設定します。</summary>
        public double SizeScale
        {
            get { return _sizeScale; }
            set { SetAndRaisePropertyChanged(ref _sizeScale, value); }
        }

    }
}
