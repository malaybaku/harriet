using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;

using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>声種「男性1」を実装します。</summary>
    public class AquesVoiceM1 : AquesVoiceSynthesize
    {
        protected override IntPtr SyntheWave(string koe, int speed, ref int size)
        {
            return AquesTalk_Synthe(koe, speed, ref size);
        }

        protected override void FreeWave(IntPtr wavPtr)
        {
            AquesTalk_FreeWave(wavPtr);
        }

        [DllImport(AquesVoiceConstNames.PathM1, EntryPoint = AquesVoiceConstNames.AquesTalk_Synthe)]
        public extern static IntPtr AquesTalk_Synthe(string koe, int speed, ref int size);

        [DllImport(AquesVoiceConstNames.PathM1, EntryPoint = AquesVoiceConstNames.AquesTalk_FreeWave)]
        public extern static void AquesTalk_FreeWave(IntPtr wavPtr);

    }

    /// <summary>「男性1」ボイス実装のファクトリクラスです。</summary>
    [Export(typeof(IVoiceSynthesizeFactory))]
    public class AquesVoiceM1Factory : IVoiceSynthesizeFactory
    {
        public string Name { get; } = AquesVoiceConstNames.NameM1;

        public IVoiceSynthesize CreateVoiceSynthesizer() => new AquesVoiceM1();
    }

}
