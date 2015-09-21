using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using Harriet.Models.Core;
using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>音声合成用の文字列に前処理をかけるプラグインのローダーを表します。</summary>
    class TextToPronounceConverterLoader
    {
        const string TextToPronouncePluginDirectory = "TextToPronounce";
        //const string PluginDirectory = "Plugin";

        /// <summary>ファクトリパターンみたく生インスタンスの取得を禁止</summary>
        private TextToPronounceConverterLoader() { }

        [ImportMany]
        List<ITextToPronounceConverterFactory> _converters = new List<ITextToPronounceConverterFactory>();

        //private static TextToPronounceConverterLoader _loaderForThisAssembly;
        //private static TextToPronounceConverterLoader _loaderForDirectory;

        private static IReadOnlyDictionary<string, ITextToPronounceConverterFactory> _factories;

        private static List<ITextToPronounceConverterFactory> LoadDictFromThisAssembly()
        {
            var loader = new TextToPronounceConverterLoader();
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);

            return loader._converters;
        }
        private static List<ITextToPronounceConverterFactory> LoadDictFromDirectory()
        {
            Directory.CreateDirectory(Path.Combine(PluginDirectoryNames.PluginDirectory, TextToPronouncePluginDirectory));

            var loader = new TextToPronounceConverterLoader();
            var catalog = new DirectoryCatalog(Path.Combine(PluginDirectoryNames.PluginDirectory, TextToPronouncePluginDirectory));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);

            return loader._converters;
        }

        public static IReadOnlyDictionary<string, ITextToPronounceConverterFactory> LoadAvailableTextConverters()
        {
            if(_factories == null)
            {
                _factories = LoadDictFromThisAssembly()
                    .Concat(LoadDictFromDirectory())
                    .ToDictionary(p => p.Name, p => p);
            }

            return _factories;
        }

    }



    /// <summary>変換をまったく行わないテキスト前処理器を表します。</summary>
    public class EmptyTextToPronounceConverter : ITextToPronounceConverter
    {
        public const string ConverterName = "無変換(デフォルト)";

        /// <summary>テキストに前処理を行います。このクラスの場合、入力を素通しします。</summary>
        /// <param name="input">入力されたテキスト</param>
        /// <returns>変換後のテキスト</returns>
        public string Convert(string input) => input;

        //リソース無いので何もしないでOK
        public void Dispose() { }
    }

    /// <summary>空のコンバータを</summary>
    [Export(typeof(ITextToPronounceConverterFactory))]
    public class EmptyTextToPronounceConverterFactory : ITextToPronounceConverterFactory
    {
        public string Name { get; } = "無変換(デフォルト)";

        public ITextToPronounceConverter CreateConverter()
        {
            return new EmptyTextToPronounceConverter();
        }
    }

}
