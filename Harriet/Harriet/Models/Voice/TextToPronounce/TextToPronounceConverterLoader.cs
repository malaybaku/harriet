using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>音声合成用の文字列に前処理をかけるプラグインのローダーを表します。</summary>
    class TextToPronounceConverterLoader
    {
        const string PluginDirectory = "Plugin";

        /// <summary>ファクトリパターンみたく生インスタンスの取得を禁止</summary>
        private TextToPronounceConverterLoader() { }

        [ImportMany]
        List<ITextToPronounceConverter> _converters = new List<ITextToPronounceConverter>();

        /// <summary>ロード処理</summary>
        /// <returns>ロードされた音声合成の前処理コンバータ一覧</returns>
        public static List<ITextToPronounceConverter> Load()
        {
            var loader = new TextToPronounceConverterLoader();
            var catalog = new DirectoryCatalog(PluginDirectory);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);

            return loader._converters;
        }

    }

    /// <summary>変換をまったく行わないテキスト前処理器を表します。</summary>
    public class EmptyTextToPronounceConverter : ITextToPronounceConverter
    {
        /// <summary>テキストに前処理を行います。このクラスの場合、入力を素通しします。</summary>
        /// <param name="input">入力されたテキスト</param>
        /// <returns>変換後のテキスト</returns>
        public string Convert(string input) => input;
    }

}
