using System;

namespace Harriet.Models.Core
{
    /// <summary>キャラ変更指示を出せるインターフェースを表します。</summary>
    public interface ICharacterSelector
    {
        /// <summary>キャラクターが選ばれたときに発生します。</summary>
        event EventHandler<CharacterSelectedEventArgs> CharacterSelected;
    }

    /// <summary>キャラ選択を表すイベント引数です。</summary>
    public class CharacterSelectedEventArgs : EventArgs
    {
        /// <summary>キャラ名を用いてインスタンスを初期化します。</summary>
        /// <param name="characterName">キャラ名</param>
        public CharacterSelectedEventArgs(string characterName)
        {
            CharacterName = characterName;
        }

        /// <summary>選択されたキャラクターの名前を取得します。</summary>
        public string CharacterName { get; }
    }

}
