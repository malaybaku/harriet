using System.ComponentModel;

namespace Harriet.Models.Chat
{
    /// <summary>キャラの表示名を定義します。</summary>
    public interface IShownCharacterNameSetting : INotifyPropertyChanged
    {
        /// <summary>キャラの表示名を取得、設定します。</summary>
        string CharacterName { get; set; }
    }

    /// <summary>表示されるキャラ名を表します。</summary>
    public class ShownCharacterNameSetting : HarrietNotifiableModelBase, IShownCharacterNameSetting
    {
        private string _characterName = string.Empty;
        /// <summary>キャラの表示名を取得、設定します。</summary>
        public string CharacterName
        {
            get { return _characterName; }
            set { SetAndRaisePropertyChanged(ref _characterName, value); }
        }
    }

}
