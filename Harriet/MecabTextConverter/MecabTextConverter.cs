using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.Composition;

using NMeCab;

using HarrietModelInterface;

namespace MecabTextConverter
{
    /// <summary>形態素解析ライブラリNMeCabを用いて平文/発音文変換を行う変換器を表します。</summary>
    public class MecabTextConverter : ITextToPronounceConverter
    {
        /// <summary>インスタンスを初期化します。</summary>
        internal MecabTextConverter()
        {
            InitializeMeCabTagger();
        }

        /// <summary>平文をAquesTalkでの発音に適したテキストに変換します。</summary>
        /// <param name="input">平文(漢字/かな/カナ交じり文)</param>
        /// <returns>AquesTalkで発声可能な文字列</returns>
        public string Convert(string input)
        {
            if(_meCabTagger == null)
            {
                InitializeMeCabTagger();
            }

            var result = new StringBuilder();

            for (var node = _meCabTagger.ParseToNode(input).Next;
                node.Feature != null;
                node = node.Next
                )
            {
                string[] features = node.Feature.Split(',');
                string category = features[0];
                string categorySmall = features[1];
                string added = features[features.Length - 1];

                if (added == "*")
                {
                    continue;
                }

                if (NotSlashedWords.Contains(added))
                {
                    //例:「、」とか「！」といった形態素
                    //何も要らない
                }
                else if (_isAdWords(category, categorySmall))
                {
                    //例:「私は」の「は」とかの助詞っぽいやつ
                    result.Append("+");
                }
                else if (_isDependentWord(category, categorySmall))
                {
                    //例:「知ってる」の「てる」など単体で成立しないタイプの動詞/名詞
                    //何も要らない
                }
                else
                {
                    //普通の名詞や動詞
                    result.Append("/");
                }

                result.Append(added);

            }

            return result.ToString().Trim('/', '+');

        }

        public void Dispose()
        {
            if (_meCabTagger != null)
            {
                _meCabTagger.Dispose();
                _meCabTagger = null;
            }
        }

        private MeCabTagger _meCabTagger;

        private void InitializeMeCabTagger()
        {
            var param = new MeCabParam();
            param.DicDir = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"dic\ipadic"
                );

            _meCabTagger = MeCabTagger.Create(param);
        }

        #region 品詞の種類分析するための静的関数/プロパティ

        private static bool _isAdWords(string category, string categorySmall)
            => SubWordCategories.Contains(category);

        private static bool _isDependentWord(string category, string categorySmall)
            => SubWordCategoryPair.Contains(new Tuple<string, string>(category, categorySmall));

        /// <summary>単語の区切れ目にならない品詞の種類を取得</summary>
        private static string[] SubWordCategories { get; } = new[]
        {
            "助動詞",
            "助詞",
            "格助詞"
        };

        /// <summary>サブカテゴリまで見て</summary>
        private static Tuple<string, string>[] SubWordCategoryPair { get; } = new[]
        {
            new Tuple<string, string>("名詞", "非自立"),
            new Tuple<string, string>("動詞", "非自立")
        };

        /// <summary>AquesTalkの仕様を考え、記号の直前にスラッシュ("/")を入れない方がいい</summary>
        private static string[] NotSlashedWords { get; } = new[]
        {
            "。",
            "、",
            ".",
            ",",
            "！",
            "？",
            "!",
            "?"
        };

        #endregion

    }
}
