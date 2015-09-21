using System;
using System.Runtime.InteropServices;

using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>AquesTalkを使って音声合成を行うクラスを表します。</summary>
    public abstract class AquesVoiceSynthesize : IVoiceSynthesize
    {
        #region インターフェース実装

        public int Volume { get; set; } = 50;
        public int Speed { get; set; } = 100;
        public int Pitch { get; set; } = 100;

        /// <summary> 音声波形を生成する </summary>
        /// <param name="pronounce"> (株)アクエストが定める発音記号列 </param>
        /// <returns> wavファイルの内容物に相当するデータ </returns>
        public byte[] CreateWav(string pronounce)
        {
            try
            {
                int size = 0;

                IntPtr wavPtr = SyntheWave(pronounce, Speed, ref size);

                //wav生成に失敗してるケース: 具体的には[あ-んア-ン.,。、/+_]あたりの記号以外が入ってると失敗する
                if (wavPtr == IntPtr.Zero)
                {
                    //ヌルぽなので意味はないハズだが一応
                    FreeWave(wavPtr);
                    return new byte[] { };
                }

                var wav = new byte[size];
                Marshal.Copy(wavPtr, wav, 0, size);

                FreeWave(wavPtr);

                //パラメタに応じて波形処理が入る
                //FIXME: NAudio使った方がカッコいい…か？AquesTalkだけターゲットにするなら別に現状でもいいが
                WaveInfo.ChangePitch(wav, Pitch);
                WaveInfo.ChangeVolume(wav, Volume);

                return wav;
            }
            catch (ArgumentNullException)
            {
                return new byte[] { };
            }
        }

        /// <summary>静的関数のロードしかしてないので解放不要(というか解放するのムリでは)</summary>
        public void Dispose() { }

        #endregion

        /// <summary>AquesTalk_Synthe相当の操作</summary>
        protected abstract IntPtr SyntheWave(string koe, int speed, ref int size);

        /// <summary>AquesTalk_FreeWave相当の操作</summary>
        protected abstract void FreeWave(IntPtr wavPtr);

    }

}
