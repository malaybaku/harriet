using System;
using System.IO;

using IronPython.Hosting;
using IronPython.Runtime;

using Harriet.CharacterInterface;

namespace Harriet.Models.Core
{
    /// <summary> IronPythonの読み込みによってキャラクターを読み込むクラス </summary>
    public static class IronPythonCharacterLoader
    {
        /// <summary>キャラロードの操作を規定したスクリプトの名前です。</summary>
        public const string InitializeScriptName = "get_character.py";

        /// <summary>キャラを戻り値として返す関数の名前です。</summary>
        public const string CharacterLoadFunction = "get_character";

        /// <summary>キャラを定められた仕様にもとづいてロードする。失敗すると例外が飛んでくる</summary>
        public static IHarrietCharacter LoadCharacter(string characterName)
        {
            var engine = Python.CreateEngine();
            //名前参照にexeのディレクトリとキャラのディレクトリを追加
            var paths = engine.GetSearchPaths();
            paths.Add(Environment.CurrentDirectory);
            paths.Add(DirectoryNames.GetCharacterScriptDirectory(characterName));
            paths.Add(DirectoryNames.GetCharacterLoadedDirectory(characterName));
            engine.SetSearchPaths(paths);

            string path = GetInitializeScriptPath(characterName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"file '{path}' was not found: this is needed to load character data");
            }
            engine.ExecuteFile(path, engine.Runtime.Globals);

            dynamic loadFunction;
            bool result = engine.Runtime.Globals.TryGetVariable(CharacterLoadFunction, out loadFunction);
            if (!result)
            {
                throw new InvalidOperationException($"'{CharacterLoadFunction}' function does not exist in '{path}'");
            }
            PythonFunction function = loadFunction as PythonFunction;
            if (function == null)
            {
                throw new InvalidOperationException($"'{CharacterLoadFunction}' defined in '{path}' is not function");
            }
            
            IHarrietCharacter character = loadFunction() as IHarrietCharacter;
            if (character == null)
            {
                throw new InvalidOperationException($"{CharacterLoadFunction} result does not implements IHarrietCharacter");
            }

            return character;
        }

        /// <summary>指定したキャラが表示可能であるかを判定します。</summary>
        /// <returns>キャラの利用が可能そうであればtrueが帰り、そうでない場合は例外がそのまま飛んでいく</returns>
        public static bool CheckCharacterValidity(string characterName)
        {
            //NOTE: IHarrietCharacterがIDisposableなことを忘れずに。
            using (var character = LoadCharacter(characterName))
            {
                return true;
            }
        }


        private static string GetInitializeScriptPath(string characterName)
        {
            return Path.Combine(DirectoryNames.GetCharacterLoadedDirectory(characterName), InitializeScriptName);
        }

    }
}
