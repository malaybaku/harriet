using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Harriet.Models.Core
{

    /// <summary>プログラム中で使っているディレクトリ名関連のサポートを提供します。</summary>
    public static class DirectoryNames
    {
        /// <summary>キャラ情報が入るディレクトリ名</summary>
        public const string CharacterDirectory = "characters";

        /// <summary>各キャラのうち、ロードに使うdllかIronPythonスクリプトを表すディレクトリ名</summary>
        public const string LoadDirectory = "core";

        /// <summary>スクリプトが含まれるディレクトリ名</summary>
        private const string ScriptDirectory = "script";

        /// <summary>キャラのロードに使うdllまたはIronPythonスクリプトが含まれるフォルダのパスを取得します。</summary>
        /// <param name="characterName">キャラ名</param>
        /// <returns>キャラのロード先のパス</returns>
        public static string GetCharacterLoadedDirectory(string characterName)
        {
            return Path.Combine(
                Environment.CurrentDirectory, CharacterDirectory, characterName, LoadDirectory
                );
        }

        /// <summary>キャラのスクリプトが含まれるディレクトリへのパスを取得する</summary>
        /// <param name="characterName">キャラ名</param>
        /// <param name="isAbsolutePath">絶対パスで取得するかどうか</param>
        /// <returns>キャラのスクリプトが含まれるディレクトリへのパス</returns>
        public static string GetCharacterScriptDirectory(string characterName, bool isAbsolutePath=true)
        {
            if (!isAbsolutePath)
            {
                return Path.Combine(CharacterDirectory, characterName, ScriptDirectory);
            }
            else
            {
                return Path.Combine(
                    Environment.CurrentDirectory,
                    CharacterDirectory, 
                    characterName, 
                    ScriptDirectory
                    );
            }

        }

        /// <summary> キャラのルートディレクトリを取得する </summary>
        /// <param name="characterName">キャラ名</param>
        /// <returns>ルートディレクトリのパス</returns>
        public static string GetCharacterDirectory(string characterName)
        {
            return Path.Combine(CharacterDirectory, characterName);
        }

        /// <summary> 利用可能なキャラ候補の一覧を取得します。 </summary>
        /// <returns> キャラ名の一覧 </returns>
        public static string[] AvailableCharacters
        {
            get 
            {
                return Directory.GetDirectories(CharacterDirectory)
                   .Select(Path.GetFileName)
                   .ToArray(); 
            }
        }

        /// <summary>利用可能キャラについて、メニューへの表示名がフォルダ名と違う場合、、その対応関係を取得します。</summary>
        /// <returns>
        /// フォルダ名をキー、表示名を値とした辞書。
        /// 表示名が特別に設定されてない場合、値にはキーがそのまま入る
        /// </returns>
        public static Dictionary<string, string> GetAvailableDisplayNames()
        {
            var keys = AvailableCharacters;
            return keys.ToDictionary(
                k => k,
                k =>
            {
                string nameTxtFile = Path.Combine(GetCharacterDirectory(k), "name.txt");
                if(File.Exists(nameTxtFile))
                {
                    return File.ReadAllLines(nameTxtFile, Encoding.UTF8)[0];
                }
                else
                {
                    return k;
                }
            });
        }

    }
}
