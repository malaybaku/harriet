using System.ComponentModel;
using Harriet.Models.Chat;

namespace Harriet.ViewModels
{
    /// <summary>表示されるキャラ名のビューモデルを表します。</summary>
    public class ShownCharacterNameViewModel : HarrietViewModelBase
    {
        /// <summary>モデルの設定をもとにビューモデルを初期化します。</summary>
        /// <param name="setting">モデルでの表示名設定</param>
        public ShownCharacterNameViewModel(IShownCharacterNameSetting setting)
        {
            CharacterName = setting.CharacterName;

            PropertyChanged += (_, __) => setting.CharacterName = CharacterName;
            PropertyChangedEventManager.AddHandler(
                setting,
                (_, __) => CharacterName = setting.CharacterName, 
                nameof(CharacterName)
                );
        }

        private string _characterName = string.Empty;
        /// <summary>表示されるキャラ名を取得、設定します。</summary>
        public string CharacterName
        {
            get { return _characterName; }
            set { SetAndRaisePropertyChanged(ref _characterName, value); }
        }

    }
}
