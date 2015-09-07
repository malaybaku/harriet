using System;
using System.Threading.Tasks;

namespace Harriet.Models.Voice
{
    /// <summary>音声出力の運用を定義します。</summary>
    public interface IVoiceOperator : IDisposable
    {
        /// <summary>発音をもとに発声します。</summary>
        /// <param name="pronounce">発音を表す文字列</param>
        /// <param name="useLipSynch">リップシンクの有効、無効を指定します。</param>
        /// <returns>発声処理を表すタスク</returns>
        Task PlayByPronounce(string pronounce, bool useLipSynch);
 
        /// <summary>音声ファイルをもとに発声します。</summary>
        /// <param name="wavpath">音声ファイルのパス</param>
        /// <param name="useLipSynch">リップシンクの有効、無効を指定します。</param>
        /// <returns>発声処理を表すタスク</returns>
        Task PlayByFile(string wavpath, bool useLipSynch);

        /// <summary>発声を中止します。</summary>
        void Stop();

        /// <summary>リップシンクが必要になったときに発生します。</summary>
        event EventHandler<LipSynchEventArgs> LipSynchRequested;
    }
}
