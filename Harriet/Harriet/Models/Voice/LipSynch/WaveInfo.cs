using System;
using System.Collections.Generic;
using System.Linq;

namespace Harriet.Models.Voice
{

    /// <summary>
    /// 音声データの中身を見たり変更したりする為のクラス
    /// </summary>
    class WaveInfo
    {
        #region 音量量子化に使う閾値の定数群、直感で決めてる

        //FIXME: どっかで設定可能にしてもいいでしょうコレ。

        public const double Thresh0 = 0.02;
        public const double Thresh1 = 0.04;
        public const double Thresh2 = 0.06;
        public const double Thresh3 = 0.08;
        public const double Thresh4 = 0.10;

        #endregion

        public WaveInfo(byte[] wav)
        {
            //header = new byte[36];
            //44バイトとはデータ前のヘッダのバイト数で、AquesTalkを使う間は固定
            header = new byte[44];
            GetInfoFrom(wav);
        }

        /// <summary> 音声データのヘッダから情報を読み取る </summary>
        /// <param name="wav"></param>
        public void GetInfoFrom(byte[] wav)
        {
            //Array.Copy(wav, header, 36);
            Array.Copy(wav, header, 44);

            FormatId = GetUshortFrom(header[20], header[21]);
            Channel = GetUshortFrom(header[22], header[23]);

            SamplingRate = GetUintFrom(header[24], header[25], header[26], header[27]);
            BytePerSec = GetUintFrom(header[28], header[29], header[30], header[31]);

            BitPerSample = GetUshortFrom(header[34], header[35]);

            DataSize = GetUintFrom(header[40], header[41], header[42], header[43]);


            PlayTime = (double)(DataSize) / BytePerSec;
        }

        /// <summary> 音声データのヘッダに情報を書き込む </summary>
        /// <param name="wav"></param>
        public void SetInfoTo(byte[] wav)
        {
            byte[] newHeader = MakeHeader();

            Array.Copy(newHeader, wav, 36);
        }

        byte[] header;

        /// <summary> フォーマットID番号。AquesTalkでは16(0x0010)で固定 </summary>
        public ushort FormatId { get; set; }

        /// <summary> チャネル数(ステレオ:2, モノラル:1)。AquesTalkではモノラルで固定 </summary>
        public ushort Channel { get; set; }

        /// <summary> サンプリング周波数, たぶん44100Hz </summary>
        public uint SamplingRate { get; set; }

        /// <summary> 1秒あたりに読み込むバイト数, デフォ値はあるが速度調整の時に変更したりする </summary>
        public uint BytePerSec { get; set; }

        /// <summary> 音声データの量、実際のファイルサイズはコレにヘッダ分の44を足したものになる </summary>
        public uint DataSize { get; set; }

        /// <summary> 音声の再生時間。単位は秒 </summary>
        public double PlayTime { get; private set; }

        /// <summary> 量子化ビット数 </summary>
        public ushort BitPerSample { get; set; }



        /// <summary>
        /// ピッチを変更する。読み上げ速度と声の高さが変わる
        /// CAUTION: AquesTalk以外の音声データに対応させてない
        /// </summary>
        /// <param name="wav">音声データ</param>
        /// <param name="pitch">ピッチ(100が基準値)</param>
        public static void ChangePitch(byte[] wav, int pitch)
        {
            var wInfo = new WaveInfo(wav);
            float rate = pitch / 100.0f;
            wInfo.BytePerSec = (uint)(wInfo.BytePerSec * rate);
            wInfo.SamplingRate = (uint)(wInfo.SamplingRate * rate);
            wInfo.SetInfoTo(wav);
        }

        /// <summary>
        /// 音声の音量を調整する。現在はAquesTalkの出力wav以外に対応させる気がないのでハードコーディングが多い
        /// ハードコーディングの具体的な内容はコード内を見よ。
        /// </summary>
        /// <param name="wav">音声データ</param>
        /// <param name="rate">音量(100が標準値)</param>
        public static void ChangeVolume(byte[] wav, int rate = 100)
        {
            float rf = rate / 100.0f;

            //AquesTalkのwaveファイルはヘッダが36 + 'data' '(データのサイズのuint)' で音声本体の前に44byteある
            //その後には量子化ビット数16bitで数値が並んでいるので2byteずつ持って来ればよい
            for (int i = 44; i + 1 < wav.Length; i += 2)
            {
                //short currentVol = GetShortFrom(wav[i], wav[i + 1]);
                //currentVol = (short)(currentVol * rf);
                //byte[] modified = BitConverter.GetBytes(currentVol);

                //wav[i] = modified[0];
                //wav[i + 1] = modified[1];

                byte[] bytes = BitConverter.GetBytes(
                        (short)(GetShortFrom(wav[i], wav[i + 1]) * rf));
                Array.Copy(bytes, 0, wav, i, 2);
            }

        }



        /// <summary>
        /// wavを時間でぶつ切りにして音量の配列を返す。口パク連動サポート。
        /// </summary>
        /// <param name="wav">音声データ(.wav)</param>
        /// <param name="interval">時間区切り幅</param>
        /// <returns></returns>
        private static double[] Volumes(byte[] wav, double interval)
        {
            var wInfo = new WaveInfo(wav);
            var result = new List<double>();

            for (int i = 1; (i + 1) * interval < wInfo.PlayTime; i++)
            {
                result.Add(VolumeAt(wav, i * interval, interval));
            }

            //
            //File.WriteAllLines("wavMeans.dat", result.Select(v => v.ToString()).ToArray());

            return result.ToArray();
        }

        /// <summary>
        /// 0から5までで量子化された音量の段階値を吐き出す。
        /// この値は口パクに使われることを想定してる
        /// </summary>
        /// <param name="wav"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static int[] VolumesDiscrete(byte[] wav, double interval)
        {
            return Volumes(wav, interval)
                .Select(v =>
                    v < Thresh0 ? 0 :
                    v < Thresh1 ? 1 :
                    v < Thresh2 ? 2 :
                    v < Thresh3 ? 3 :
                    v < Thresh4 ? 4 : 5
                ).ToArray();
        }

        /// <summary>
        /// ファイルのある位置での音量を取得する
        /// </summary>
        /// <param name="t">時刻</param>
        /// <param name="wav">時刻の周辺幅</param>
        /// <param name="width">周辺値をとってくる時間幅(秒)</param>
        /// <returns>音量の2乗平均値</returns>
        private static double VolumeAt(byte[] wav, double t, double width)
        {
            //2015/5/23変更: 任意のブロックサイズやBit Per Sampleに対応出来るようにする
            var wInfo = new WaveInfo(wav);
            int bytePerSample = wInfo.BitPerSample / 8;
            int min = (int)(t * wInfo.BytePerSec);
            int max = (int)((t + width) * wInfo.BytePerSec);

            min = Math.Max(min, 0);
            max = Math.Min(max, wav.Length - 44 - bytePerSample);

            //スタート位置のアレイをきちんとする
            min -= min % bytePerSample;
            max -= max % bytePerSample;

            //44はヘッダの長さ
            min += 44;
            max += 44;

            //無いと思うがtやwidthの設定に対してロバストにしておく
            if (min >= max) return 0.0;

            var volumes = new float[(max - min) / bytePerSample];

            int index = 0;
            for (int i = min; i < max; i += bytePerSample)
            {
                //HACK: BigEndian対応のためbyte数毎に対応する。
                //いやホントは一般化できる(最上位ビット見て余剰ビット埋めればいい)んだけど…
                if (bytePerSample == 1)
                {
                    volumes[index] = wav[i] / 128.0f;
                }
                else if (bytePerSample == 2)
                {
                    volumes[index] = GetShortFrom(wav[i], wav[i + 1]) / 32768.0f;
                }
                else if (bytePerSample == 3)
                {
                    var wavValue = new byte[3];
                    Array.Copy(wav, i, wavValue, 0, 3);
                    volumes[index] = GetIntFrom3Byte(wavValue) / 8388608.0f;
                }
                index++;
            }

            return Math.Sqrt(volumes.Sum(v => v * v) / volumes.Length);

        }

        /// <summary> プロパティからヘッダを再構成する </summary>
        private byte[] MakeHeader()
        {
            var result = new byte[36];
            Array.Copy(header, result, 36);


            if (BitConverter.IsLittleEndian)
            {
                result[21] = (byte)(FormatId >> 8);
                result[20] = (byte)FormatId;

                result[23] = (byte)(Channel >> 8);
                result[22] = (byte)(Channel);

                result[27] = (byte)(SamplingRate >> 24);
                result[26] = (byte)(SamplingRate >> 16);
                result[25] = (byte)(SamplingRate >> 8);
                result[24] = (byte)SamplingRate;

                result[31] = (byte)(BytePerSec >> 24);
                result[30] = (byte)(BytePerSec >> 16);
                result[29] = (byte)(BytePerSec >> 8);
                result[28] = (byte)BytePerSec;

                result[35] = (byte)(BitPerSample >> 8);
                result[34] = (byte)BitPerSample;
            }
            else
            {
                result[20] = (byte)(FormatId >> 8);
                result[21] = (byte)FormatId;

                result[22] = (byte)(Channel >> 8);
                result[23] = (byte)(Channel);

                result[24] = (byte)(SamplingRate >> 24);
                result[25] = (byte)(SamplingRate >> 16);
                result[26] = (byte)(SamplingRate >> 8);
                result[27] = (byte)SamplingRate;

                result[28] = (byte)(BytePerSec >> 24);
                result[29] = (byte)(BytePerSec >> 16);
                result[30] = (byte)(BytePerSec >> 8);
                result[31] = (byte)BytePerSec;

                result[34] = (byte)(BitPerSample >> 8);
                result[35] = (byte)BitPerSample;
            }

            return result;
        }

        //NOTE: 引数はバイト列として並んだまんまの数字であることを期待する
        private static short GetShortFrom(byte b1, byte b2)
        {
            var bytes = new byte[] { b1, b2 };
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt16(bytes, 0);
        }
        private static ushort GetUshortFrom(byte b1, byte b2)
        {
            var bytes = new byte[] { b1, b2 };
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt16(bytes, 0);

        }
        private static int GetIntFrom(byte[] data)
        {
            var dataCopy = new byte[data.Length];
            Array.Copy(data, dataCopy, data.Length);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataCopy);
            }
            return BitConverter.ToInt32(dataCopy, 0);
        }
        private static uint GetUintFrom(byte b1, byte b2, byte b3, byte b4)
        {
            var bytes = new byte[] { b1, b2, b3, b4 };
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary> Little Endian形式の3バイト整数を普通の整数に直す </summary>
        private static int GetIntFrom3Byte(byte[] data)
        {
            var dataCopy = new byte[4];
            Array.Copy(data, dataCopy, 3);
            if (data[2] > 127)
            {
                //負数なので最上位も補数表現で埋める
                dataCopy[3] = (byte)0xff;
            }
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataCopy);
            }
            return BitConverter.ToInt32(dataCopy, 0);
        }
    }
}
