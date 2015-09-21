using System;
using System.ComponentModel.Composition;
using HarrietModelInterface;

namespace MecabTextConverter
{
    /// <summary>形態素解析ライブラリNMeCabを用いて平文/発音文変換を行う変換器を生成するクラスを表します。</summary>
    [Export(typeof(ITextToPronounceConverterFactory))]
    public class MeCabTextConverterFactory : ITextToPronounceConverterFactory
    {
        public string Name { get; } = "MeCab";

        public ITextToPronounceConverter CreateConverter() => new MecabTextConverter();
    }
}
