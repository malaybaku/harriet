using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;

using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>声種「機械」を実装します。</summary>
    public class AquesVoiceMachine : AquesVoiceSynthesize
    {
        protected override IntPtr SyntheWave(string koe, int speed, ref int size)
        {
            return AquesTalk_Synthe(koe, speed, ref size);
        }

        protected override void FreeWave(IntPtr wavPtr)
        {
            AquesTalk_FreeWave(wavPtr);
        }

        [DllImport(AquesVoiceConstNames.PathDvd, EntryPoint = AquesVoiceConstNames.AquesTalk_Synthe)]
        public extern static IntPtr AquesTalk_Synthe(string koe, int speed, ref int size);

        [DllImport(AquesVoiceConstNames.PathDvd, EntryPoint = AquesVoiceConstNames.AquesTalk_FreeWave)]
        public extern static void AquesTalk_FreeWave(IntPtr wavPtr);
    }

    /// <summary>「機械」ボイス実装のファクトリクラスです。</summary>
    [Export(typeof(IVoiceSynthesizeFactory))]
    public class AquesVoiceMachineFactory : IVoiceSynthesizeFactory
    {
        public string Name { get; } = AquesVoiceConstNames.NameDvd;

        public IVoiceSynthesize CreateVoiceSynthesizer() => new AquesVoiceMachine();
    }

}
