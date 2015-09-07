
namespace Harriet.Models.Voice
{
    /// <summary>音声波形の生成を定義します。</summary>
    public interface IVoiceSynther
    {
        /// <summary> 音声波形を生成する </summary>
        /// <param name="pronounce"> (株)アクエストが定める発音記号列 </param>
        /// <returns> wavファイルの内容物に相当するデータ </returns>
        byte[] CreateWav(string pronounce);
    }
}
