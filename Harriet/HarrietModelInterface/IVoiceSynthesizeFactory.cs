
namespace HarrietModelInterface
{
    public interface IVoiceSynthesizeFactory
    {
        /// <summary>声の種類を表す文字列を取得します(Harrietの設定画面に表示されます)</summary>
        string Name { get; }

        /// <summary>音声合成器を生成します。</summary>
        /// <returns>音声合成器</returns>
        IVoiceSynthesize CreateVoiceSynthesizer();

    }
}
