using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using Harriet.Models.Core;
using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>音声合成器のローダーを表します。</summary>
    class VoiceSynthesizerLoader
    {
        const string VoicePluginDirectory = "Voice";

        /// <summary>ファクトリパターンみたく生インスタンスの取得を禁止</summary>
        private VoiceSynthesizerLoader() { }

        [ImportMany]
        List<IVoiceSynthesizeFactory> _converters = new List<IVoiceSynthesizeFactory>();

        /// <summary>名前のついたファクトリ一覧</summary>
        private static IReadOnlyList<IVoiceSynthesizeFactory> _factories;

        private static List<IVoiceSynthesizeFactory> LoadDictFromThisAssembly()
        {
            var loader = new VoiceSynthesizerLoader();
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);

            return loader._converters;
        }
        private static List<IVoiceSynthesizeFactory> LoadDictFromDirectory()
        {
            Directory.CreateDirectory(Path.Combine(PluginDirectoryNames.PluginDirectory, VoicePluginDirectory));

            var loader = new VoiceSynthesizerLoader();
            var catalog = new DirectoryCatalog(Path.Combine(PluginDirectoryNames.PluginDirectory, VoicePluginDirectory));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(loader);

            return loader._converters;
        }
        
        /// <summary>利用可能な音声合成器をまとめて取得します。</summary>
        /// <returns>全ての利用可能な音声合成器クラス</returns>
        public static IReadOnlyList<IVoiceSynthesizeFactory> LoadAvailableVoices()
        {
            if (_factories == null)
            {
                _factories = LoadDictFromThisAssembly()
                    .Concat(LoadDictFromDirectory())
                    .ToList()
                    .AsReadOnly();
            }

            return _factories;
        }

    }
}
