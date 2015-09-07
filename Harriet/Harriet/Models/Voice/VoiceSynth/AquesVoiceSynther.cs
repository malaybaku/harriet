using System;
using System.Runtime.InteropServices;

namespace Harriet.Models.Voice
{
    /// <summary>アクエストークを使って音声合成を行うクラスを表します。</summary>
    public class AquesVoiceSynther : IVoiceSynther
    {
        /// <summary>音声設定を用いてインスタンスを初期化します。</summary>
        /// <param name="setting">音声の諸元設定</param>
        public AquesVoiceSynther(IVoiceSetting setting)
        {
            VoiceSetting = setting;
        }

        #region 公開関数

        /// <summary> 音声波形を生成する </summary>
        /// <param name="pronounce"> (株)アクエストが定める発音記号列 </param>
        /// <returns> wavファイルの内容物に相当するデータ </returns>
        public byte[] CreateWav(string pronounce)
        {
            try
            {
                int size = 0;

                //NOTE: AquesTalk_Syntheの実行中に_voiceが変更されると
                //SyntheとFreeが整合しない可能性があるのであらかじめ固定が必要
                AquesVoice voice = GetVoiceType(VoiceSetting.VoiceType);

                //声種と速度
                IntPtr wavPtr = AquesTalk_Synthe(voice, pronounce, VoiceSetting.Speed, ref size);

                //wav生成に失敗してるケース
                if (wavPtr == IntPtr.Zero)
                {
                    //ヌルぽなので意味はないハズ
                    AquesTalk_FreeWave(voice, wavPtr);
                    return new byte[] { };
                }

                var wav = new byte[size];
                Marshal.Copy(wavPtr, wav, 0, size);

                AquesTalk_FreeWave(voice, wavPtr);

                //パラメタに応じて波形処理が入る
                WaveInfo.ChangePitch(wav, VoiceSetting.Pitch);
                WaveInfo.ChangeVolume(wav, VoiceSetting.Volume);

                return wav;
            }
            catch (ArgumentNullException)
            {
                return new byte[] { };
            }
        }

        #endregion

        public IVoiceSetting VoiceSetting { get; }

        private static IntPtr AquesTalk_Synthe(AquesVoice voice, string koe, int speed, ref int size)
        {
            switch (voice)
            {
                case AquesVoice.F1:
                    return AqSynther.Synthe_F1(koe, speed, ref size);
                case AquesVoice.F2:
                    return AqSynther.Synthe_F2(koe, speed, ref size);
                case AquesVoice.M1:
                    return AqSynther.Synthe_M1(koe, speed, ref size);
                case AquesVoice.M2:
                    return AqSynther.Synthe_M2(koe, speed, ref size);
                case AquesVoice.Robot:
                    return AqSynther.Synthe_R1(koe, speed, ref size);
                case AquesVoice.Neutral:
                    return AqSynther.Synthe_IMD1(koe, speed, ref size);
                case AquesVoice.Machine:
                    return AqSynther.Synthe_DVD(koe, speed, ref size);
                case AquesVoice.Special:
                    return AqSynther.Synthe_JGR(koe, speed, ref size);
                default:
                    throw new ArgumentException();
            }
        }

        private static void AquesTalk_FreeWave(AquesVoice voice, IntPtr wavPtr)
        {
            switch (voice)
            {
                case AquesVoice.F1:
                    AqSynther.FreeWave_F1(wavPtr);
                    return;
                case AquesVoice.F2:
                    AqSynther.FreeWave_F2(wavPtr);
                    return;
                case AquesVoice.M1:
                    AqSynther.FreeWave_M1(wavPtr);
                    return;
                case AquesVoice.M2:
                    AqSynther.FreeWave_M2(wavPtr);
                    return;
                case AquesVoice.Robot:
                    AqSynther.FreeWave_R1(wavPtr);
                    return;
                case AquesVoice.Neutral:
                    AqSynther.FreeWave_IMD1(wavPtr);
                    return;
                case AquesVoice.Machine:
                    AqSynther.FreeWave_DVD(wavPtr);
                    return;
                case AquesVoice.Special:
                    AqSynther.FreeWave_JGR(wavPtr);
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary> 旧版AquesTalkライブラリを使うための静的クラス </summary>
        private static class AqSynther
        {
            const string PathF1 = @"dll\AquesTalk\f1\AquesTalk.dll";
            const string PathF2 = @"dll\AquesTalk\f2\AquesTalk.dll";
            const string PathM1 = @"dll\AquesTalk\m1\AquesTalk.dll";
            const string PathM2 = @"dll\AquesTalk\m2\AquesTalk.dll";
            const string PathR1 = @"dll\AquesTalk\r1\AquesTalk.dll";
            const string PathImd1 = @"dll\AquesTalk\imd1\AquesTalk.dll";
            const string PathDvd = @"dll\AquesTalk\dvd\AquesTalk.dll";
            const string PathJgr = @"dll\AquesTalk\jgr\AquesTalk.dll";

            #region AquesTalk_Synthe

            [DllImport(PathF1, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_F1(string koe, int speed, ref int size);

            [DllImport(PathF2, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_F2(string koe, int speed, ref int size);

            [DllImport(PathM1, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_M1(string koe, int speed, ref int size);

            [DllImport(PathM2, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_M2(string koe, int speed, ref int size);

            [DllImport(PathR1, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_R1(string koe, int speed, ref int size);

            [DllImport(PathImd1, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_IMD1(string koe, int speed, ref int size);

            [DllImport(PathDvd, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_DVD(string koe, int speed, ref int size);

            [DllImport(PathJgr, EntryPoint = "AquesTalk_Synthe")]
            public extern static IntPtr Synthe_JGR(string koe, int speed, ref int size);

            #endregion

            #region AquesTalk_FreeWave

            [DllImport(PathF1, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_F1(IntPtr wavPtr);

            [DllImport(PathF2, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_F2(IntPtr wavPtr);

            [DllImport(PathM1, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_M1(IntPtr wavPtr);

            [DllImport(PathM2, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_M2(IntPtr wavPtr);

            [DllImport(PathR1, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_R1(IntPtr wavPtr);

            [DllImport(PathImd1, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_IMD1(IntPtr wavPtr);

            [DllImport(PathDvd, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_DVD(IntPtr wavPtr);

            [DllImport(PathJgr, EntryPoint = "AquesTalk_FreeWave")]
            public extern static void FreeWave_JGR(IntPtr wavPtr);

            #endregion

        }

        /// <summary>文字列を対応するAquesTalkの声種に変換する。</summary>
        /// <param name="voiceStr">声種を表す文字列</param>
        /// <returns>対応する声種</returns>
        private static AquesVoice GetVoiceType(string voiceStr)
        {
            if (String.IsNullOrEmpty(voiceStr)) return AquesVoice.F1;

            AquesVoice result = AquesVoice.F1;
            AquesVoice.TryParse(voiceStr, true, out result);
            return AquesVoice.F1;
        }

        /// <summary> AquesTalkの声種 </summary>
        enum AquesVoice
        {
            F1,
            F2,
            M1,
            M2,
            Robot,
            Neutral,
            Machine,
            Special
        }
    
    }
}
