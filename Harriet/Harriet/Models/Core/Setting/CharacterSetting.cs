using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;

using Harriet.Models.Chat;
using Harriet.Models.Voice;
using Harriet.Models.Scripting;

namespace Harriet.Models.Core
{
    /// <summary> キャラごとの設定を表します。 </summary>
    [DataContract]
    public class CharacterSetting
    {
        /// <summary>設定ファイルを保存するディレクトリ名です。</summary>
        public const string SettingFileDirectory = "setting";
        /// <summary>設定ファイルの名前です。</summary>
        public const string SettingFileName = "setting.xml";

        private CharacterSetting()
        {
            //何もしない: ファクトリパターンの採用
        }

        [DataMember]
        public VoiceSetting Voice { get; private set; }

        [DataMember]
        public ChatWindowTextSetting ChatWindowText { get; private set; }

        [DataMember]
        public ChatWindowColorSetting ChatWindowColor { get; private set; }

        [DataMember]
        public ChatWindowLayoutSetting ChatWindowLayout { get; private set; }

        [DataMember]
        public ScriptRoutineSetting ScriptRoutine { get; private set; }

        [DataMember]
        public ScriptUpdateSetting ScriptUpdate { get; private set; }

        [DataMember]
        public CharacterAppearanceSetting CharacterAppearance { get; private set; }

        [DataMember]
        public ShownCharacterNameSetting ShownCharacterName { get; private set; }

        [DataMember]
        public ScriptApiSetting ScriptApi { get; private set; }

        [DataMember]
        public NadeSetting Nadenade { get; private set; }

        /// <summary>キャラ名(フォルダ名)を取得、設定します</summary>
        [DataMember]
        public string CharacterName { get; private set; }

        /// <summary>キャラ名を指定して設定ファイルへのパスを取得します。</summary>
        /// <param name="characterName">キャラ名</param>
        /// <returns>設定ファイルへのパス文字列</returns>
        private static string GetSettingFilePath(string characterName)
        {
            string dir = Path.Combine(
                DirectoryNames.GetCharacterDirectory(characterName),
                SettingFileDirectory
                );
            Directory.CreateDirectory(dir);

            return Path.Combine(dir, SettingFileName);
        }

        /// <summary>現在の設定をファイルに書き込みます。</summary>
        internal void Save()
        {
            string path = GetSettingFilePath(CharacterName);

            //念のためエンコード設定、なくてもいいか？
            var xmlSetting = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
            };

            using (var xw = XmlWriter.Create(path, xmlSetting))
            {
                var serializer = new DataContractSerializer(typeof(CharacterSetting));
                serializer.WriteObject(xw, this);
            }
        }

        /// <summary>ファイルから設定を読み込むか、既定の設定を取得します。</summary>
        /// <returns>設定一覧</returns>
        internal static CharacterSetting Load(string characterName)
        {
            string path = GetSettingFilePath(characterName);

            try
            {
                using (var xr = XmlReader.Create(path))
                {
                    var serializer = new DataContractSerializer(typeof(CharacterSetting));
                    CharacterSetting result = serializer.ReadObject(xr) as CharacterSetting;
                    result.CharacterName = characterName;
                    return result;
                }
            }
            catch (Exception)
            {
                //例外はファイルが無い、またはセーブ時のなんかの不具合によって生じる
                return CreateDefault(characterName);
            }
        }

        /// <summary>デフォルト設定を生成します。</summary>
        /// <param name="characterName">生成対象となるキャラの名前</param>
        /// <returns>キャラ名を包含したデフォルト設定</returns>
        internal static CharacterSetting CreateDefault(string characterName)
        {
            var result = new CharacterSetting();
            result.CharacterName = characterName;

            result.CharacterAppearance = new CharacterAppearanceSetting();

            result.Voice = new VoiceSetting();

            result.ChatWindowText = new ChatWindowTextSetting();
            result.ChatWindowColor = new ChatWindowColorSetting();
            result.ChatWindowLayout = new ChatWindowLayoutSetting();

            result.ShownCharacterName = new ShownCharacterNameSetting();

            result.ScriptApi = new ScriptApiSetting();
            result.ScriptRoutine = new ScriptRoutineSetting();
            result.ScriptUpdate = new ScriptUpdateSetting();

            result.Nadenade = new NadeSetting();

            return result;
        }

    }
}
