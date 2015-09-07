namespace HarrietModelInterface
{
    /// <summary>音声合成に必要な文字列変換処理を定義します、</summary>
    public interface ITextToPronounceConverter
    {
        /// <summary>平文を適宜、発音に適した形に変換します。</summary>
        /// <param name="input">平文(漢字/仮名/アルファベット等混じった一般の文字列)</param>
        /// <returns>想定する音声合成エンジンに適した発音用の文字列出力</returns>
        string Convert(string input);
    }
}
