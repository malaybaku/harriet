
using System;

namespace HarrietModelInterface
{
    /// <summary>音声波形の生成を定義します。</summary>
    public interface IVoiceSynthesize : IDisposable
    {
        /// <summary>テキストから音声波形を生成する</summary>
        /// <param name="text">
        /// 音声のもとになるテキスト。事前にITextToPronounceConverterによって
        /// 平文から前処理が行われたものが渡されるが、不正入力のケースも当然あるので注意
        /// </param>
        /// <returns>wavファイルの中身に相当するデータ</returns>
        byte[] CreateWav(string text);

        /// <summary>音量(0～300くらい)</summary>
        int Volume { set; }

        /// <summary>音程が変更しない形での速度設定(50～200くらい)</summary>
        int Speed { set; }

        /// <summary>音程ごと変わる形の速度設定(50～200くらい)</summary>
        int Pitch { set; }
    }
}
