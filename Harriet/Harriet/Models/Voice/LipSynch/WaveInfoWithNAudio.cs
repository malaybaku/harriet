using System.IO;

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harriet.Models.Voice
{
    class WaveInfoWithNAudio
    {
        #region 音量量子化に使う閾値の定数群、直感で決めてる

        //FIXME: どっかで設定可能にしてもいいでしょうコレ。

        public const double Thresh0 = 0.02;
        public const double Thresh1 = 0.04;
        public const double Thresh2 = 0.06;
        public const double Thresh3 = 0.08;
        public const double Thresh4 = 0.10;

        #endregion

        public static double GetDurationMillisec(byte[] wav)
        {
            using (var ms = new MemoryStream(wav))
            using (var fr = new WaveFileReader(ms))
            {
                return fr.TotalTime.TotalMilliseconds;
            }
        }

        public static int[] GetVolumesDiscrete(byte[] wav, double intervalSec)
        {
            using (var ms = new MemoryStream(wav))
            using (var fr = new WaveFileReader(ms))
            {
                var totalTime = fr.TotalTime;

                int[] result = new int[(int)(totalTime.TotalSeconds / intervalSec)];
                for (int i = 0; i < result.Length; i++)
                {
                    var normalizedVolumes = new List<float>();
                    var nextIntervalStart = TimeSpan.FromSeconds((i + 1) * intervalSec);
                    if(nextIntervalStart > totalTime)
                    {
                        nextIntervalStart = totalTime;
                    }

                    while (fr.CurrentTime < nextIntervalStart)
                    {
                        float[] volumes = fr.ReadNextSampleFrame();
                        foreach (var volume in volumes)
                        {
                            normalizedVolumes.Add(volume);
                        }
                    }

                    //見ての通り二乗値を平均して平方根を取る
                    double v = Math.Sqrt(normalizedVolumes
                        .Select(n => n * n)
                        .Sum() / normalizedVolumes.Count
                        );

                    result[i] = v < Thresh0 ? 0 :
                                v < Thresh1 ? 1 :
                                v < Thresh2 ? 2 :
                                v < Thresh3 ? 3 :
                                v < Thresh4 ? 4 : 5;

                }

                return result;
            }
        }

        /// <summary>指定したパーセンテージで音量を増減させた新しいwavデータを生成します。</summary>
        /// <param name="wav">元のwavデータ</param>
        /// <param name="factorPercent">音量のパーセンテージ(0～200くらいを想定)</param>
        /// <returns>音量を変更したあとのwavデータ</returns>
        public static byte[] ChangeVolume(byte[] wav, int factorPercent)
        {
            var result = new byte[wav.Length];

            using (var msIn = new MemoryStream(wav))
            using (var fr = new WaveFileReader(msIn))
            using (var msOut = new MemoryStream(result))
            using (var fw = new WaveFileWriter(msOut, fr.WaveFormat))
            {
                var allVolumes = new List<float>();
                while (fr.CurrentTime < fr.TotalTime)
                {
                    var vs = fr.ReadNextSampleFrame();
                    foreach (var v in vs)
                    {
                        allVolumes.Add(v);
                    }
                }

                fw.WriteSamples(
                    allVolumes.Select(v => v * factorPercent / 100.0f).ToArray(),
                    0, 
                    allVolumes.Count
                    );
            }

            return result;
        }

    }
}
