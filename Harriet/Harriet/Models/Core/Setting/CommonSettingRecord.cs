using System;
using System.IO;
using System.Xml.Serialization;

namespace Harriet.Models
{
    /// <summary> キャラ依存ではない共通設定を表します。 </summary>
    public class CommonSettingRecord
    {
        /// <summary>既定値でインスタンスを初期化します。</summary>
        public CommonSettingRecord()
        {
            //NOTE: β版でプロ生ちゃんを既定キャラにしてたときの名残
            //ほんとはオリキャラをデフォルトキャラにし、この辺がライセンス的にクリーンである方がいい…のだけど…
            CharacterName = "pronama";
        }

        private const string CommonSettingFilename = "harriet_setting.xml";

        /// <summary>表示するキャラの名前を取得、設定します。</summary>
        public string CharacterName { get; set; }

        /// <summary>キャラを最前面に表示するかどうかを取得、設定します。</summary>
        public bool Topmost { get; set; }

        /// <summary> 設定をファイルへ保存する </summary>
        public void Save()
        {
            using(var sw = new StreamWriter(CommonSettingFilename))
            {
                new XmlSerializer(typeof(CommonSettingRecord)).Serialize(sw, this);
            }           
        }

        /// <summary> 
        /// 設定ファイルから設定を取得する。
        /// 設定ファイルが存在しない場合は既定値が返される
        /// </summary>
        /// <returns> ファイルから取得した設定、あるいはデフォルト設定 </returns>
        public static CommonSettingRecord Load()
        {
            try
            {
                using (var sr = new StreamReader(CommonSettingFilename))
                {
                    return new XmlSerializer(typeof(CommonSettingRecord))
                        .Deserialize(sr) as CommonSettingRecord;
                }
            }
            catch (Exception)
            {
                var result = new CommonSettingRecord();
                try
                {
                    result.Save();
                }
                catch(Exception)
                {

                }
                return result;
            }
        }

    }
}
