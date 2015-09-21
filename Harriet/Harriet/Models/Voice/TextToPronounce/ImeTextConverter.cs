using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

using Livet;
using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    [Export(typeof(ITextToPronounceConverter))]
    public class ImeTextConverter : ITextToPronounceConverter
    {
        public string Name => "MS IME";

        public ImeTextConverter()
        {
            _imeLanguage = new ImeLanguage();
        }

        public string Convert(string input)
        {
            if(_imeLanguage == null)
            {
                _imeLanguage = new ImeLanguage();
            }

            return DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                string result = _imeLanguage.GetYomi(input);

                if (ReplaceAlphabetToKatakana)
                {
                    result = ConvertAlphabetToKatakana(result);
                }

                if (RemoveCharsNotSupportedInAquesTalk)
                {
                    result = Regex.Replace(result, @"[^ぁ-んァ-ン、。！？,.!?/+_]", "");
                }

                return result;
            });
        }

        /// <summary>大文字アルファベットをエー、ビーなどの発音に置き換えるかどうかを取得、設定します。</summary>
        public bool ReplaceAlphabetToKatakana { get; set; } = true;

        /// <summary>AquesTalkの発音記号列規則に則ってない文字を消去するかを取得、設定します。</summary>
        public bool RemoveCharsNotSupportedInAquesTalk { get; set; } = true;

        public void Dispose()
        {
            if(_imeLanguage != null)
            {
                _imeLanguage.Dispose();
                _imeLanguage = null;
            }
        }

        /// <summary>[a-z]の文字列は消去、[A-Z]はエー、ビー、...、ゼットという読みに直す</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ConvertAlphabetToKatakana(string input)
        {
            string result = Regex.Replace(input, "[ａ-ｚ]", "");

            return result.Replace("A", "エー")
                .Replace("B", "ビー")
                .Replace("C", "シー")
                .Replace("D", "ディー")
                .Replace("E", "イー")
                .Replace("F", "エフ")
                .Replace("G", "ジー")
                .Replace("H", "エイチ")
                .Replace("I", "アイ")
                .Replace("J", "ジェー")
                .Replace("K", "ケー")
                .Replace("L", "エル")
                .Replace("M", "エム")
                .Replace("N", "エヌ")
                .Replace("O", "オー")
                .Replace("P", "ピー")
                .Replace("Q", "キュー")
                .Replace("R", "アール")
                .Replace("S", "エス")
                .Replace("T", "ティー")
                .Replace("U", "ユー")
                .Replace("V", "ブイ")
                .Replace("W", "ダブリュー")
                .Replace("X", "エックス")
                .Replace("Y", "ワイ")
                .Replace("Z", "ゼット");
        }

        private ImeLanguage _imeLanguage;

    }
}
