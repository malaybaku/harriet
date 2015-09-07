using System;
using System.Threading.Tasks;

namespace Harriet.Models.Scripting
{
    /// <summary>同時に1つまでのスクリプト読み込みが行えるインターフェースを表します。</summary>
    public interface IScriptReader : IDisposable
    {

        /// <summary>現在スクリプトを実行中かを取得します。</summary>
        bool IsExecutingScript { get; }

        /// <summary>キャンセル可能なスクリプト読み込みを開始します。</summary>
        /// <param name="filename">スクリプトのパス</param>
        Task ReadAsync(string filename);

        /// <summary>スクリプトの読み込みをキャンセルします。</summary>
        void CancelRead();

    }
}
