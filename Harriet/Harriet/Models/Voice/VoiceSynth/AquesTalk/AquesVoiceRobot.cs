using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;

using HarrietModelInterface;

namespace Harriet.Models.Voice
{
    /// <summary>声種「ロボット」を実装します。</summary>
    public class AquesVoiceRobot : AquesVoiceSynthesize
    {
        protected override IntPtr SyntheWave(string koe, int speed, ref int size)
        {
            return AquesTalk_Synthe(koe, speed, ref size);
        }

        protected override void FreeWave(IntPtr wavPtr)
        {
            AquesTalk_FreeWave(wavPtr);
        }

        [DllImport(AquesVoiceConstNames.PathR1, EntryPoint = AquesVoiceConstNames.AquesTalk_Synthe)]
        public extern static IntPtr AquesTalk_Synthe(string koe, int speed, ref int size);

        [DllImport(AquesVoiceConstNames.PathR1, EntryPoint = AquesVoiceConstNames.AquesTalk_FreeWave)]
        public extern static void AquesTalk_FreeWave(IntPtr wavPtr);
    }

    /// <summary>「ロボット」ボイス実装のファクトリクラスです。</summary>
    [Export(typeof(IVoiceSynthesizeFactory))]
    public class AquesVoiceRobotFactory : IVoiceSynthesizeFactory
    {
        public string Name { get; } = AquesVoiceConstNames.NameR1;

        public IVoiceSynthesize CreateVoiceSynthesizer() => new AquesVoiceRobot();
    }

}
