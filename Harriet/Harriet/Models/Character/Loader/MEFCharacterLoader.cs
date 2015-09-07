using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Harriet.CharacterInterface;

namespace Harriet.Models.Core
{
    /// <summary>Managed Extension Managerを用いるキャラクターのローダーを表します。</summary>
    class MEFCharacterLoader
    {
        /// <summary>他クラスから直接インスタンス見る需要無し。</summary>
        private MEFCharacterLoader() { }

        [Import]
        private IHarrietCharacter character = default(IHarrietCharacter);

        /// <summary>キャラをロードし、インスタンスを返します。</summary>
        /// <param name="characterName">キャラの名前</param>
        /// <returns>キャラのインスタンス</returns>
        public static IHarrietCharacter Load(string characterName)
        {
            var loader = new MEFCharacterLoader();

            string path = DirectoryNames.GetCharacterLoadedDirectory(characterName);

            var catalog = new DirectoryCatalog(path);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);
            return loader.character;
        }

        /// <summary>キャラが有効にロード可能かを確認します。失敗すると例外が生じます。</summary>
        /// <param name="characterName">キャラの名前</param>
        public static void CheckValidity(string characterName)
        {
            //キャラクタはIDisposableなので成功時にDisposeする必要があることに注意
            using (var c = Load(characterName))
            {
            }
        }

    }

}
