
namespace HarrietModelInterface
{
    public interface ITextToPronounceConverterFactory
    {
        /// <summary>コンバータの名前を取得します(Harrietの設定画面に表示されます)</summary>
        string Name { get; }

        /// <summary>コンバータを生成します。</summary>
        /// <returns>平文/発音変換器</returns>
        ITextToPronounceConverter CreateConverter();
    }
}
