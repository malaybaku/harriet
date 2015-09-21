using System.ComponentModel.Composition;
using HarrietModelInterface;

namespace Harriet.Models.Voice
{

    [Export(typeof(ITextToPronounceConverterFactory))]
    public class ImeTextConverterFactory : ITextToPronounceConverterFactory
    {
        public string Name { get; } = "MS IME";

        public ITextToPronounceConverter CreateConverter() => new ImeTextConverter();
    }
}
