using System.ComponentModel;
using Harriet.Models;

namespace Harriet.ViewModels
{
    /// <summary>キャラクターの見た目に関する設定のビューモデルを表します。</summary>
    public class CharacterAppearanceViewModel : HarrietViewModelBase
    {
        public CharacterAppearanceViewModel(ICharacterAppearanceSetting setting)
        {
            SizeScale = setting.SizeScale;

            PropertyChanged += (_, __) => setting.SizeScale = SizeScale;
            PropertyChangedEventManager.AddHandler(
                setting,
                (_, __) => SizeScale = setting.SizeScale,
                nameof(SizeScale)
                );
        }

        private double _SizeScale = 1.0;
        /// <summary>キャラサイズの比率を取得、設定します。</summary>
        public double SizeScale
        {
            get { return _SizeScale; }
            set { SetAndRaisePropertyChanged(ref _SizeScale, value); }
        }
    }
}
