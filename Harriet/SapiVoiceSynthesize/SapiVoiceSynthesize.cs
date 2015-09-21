using System;
using System.IO;
using System.ComponentModel.Composition;

using SpeechLib;

using HarrietModelInterface;

namespace SapiVoiceSynthesize
{
    /// <summary>Windows標準の音声合成エンジンで発声します。</summary>
    public class SapiVoiceSynthesizer : IVoiceSynthesize
    {
        public int Pitch { get; set; }

        public int Speed { get; set; }

        public int Volume { get; set; }

        public byte[] CreateWav(string text)
        {
            Directory.CreateDirectory(TempWaveDirectoryName);

            try
            {
                //「ファイルに保存してから引きずり出し直す」という指針でやってみようまずは。
                _spStream.Open(TempWaveFileName, SpeechStreamFileMode.SSFMCreateForWrite);
                _spVoice.AudioOutputStream = _spStream;

                _spVoice.Volume = Volume < 0 ? 0 :
                                  Volume > 100 ? 100 :
                                  Volume;
                _spVoice.Speak(text);
                _spStream.Close();

                var result = File.ReadAllBytes(TempWaveFileName);
                File.Delete(TempWaveFileName);
                return result;

            }
            catch (Exception)
            {
                return new byte[] { };
            }
        }

        //SpVoiceはすぐ捨てないとマズイってほどリソース使わない(ハズ)ので適当に済ます
        public void Dispose() { }

        private SpVoice _spVoice = new SpVoice();
        private SpFileStream _spStream = new SpFileStream();
        private const string TempWaveDirectoryName = "temp";
        private const string TempWaveFileName = @"temp\haruka_tempvoice.wav";

    }

    [Export(typeof(IVoiceSynthesizeFactory))]
    public class SapiVoiceSynthesizeFactory : IVoiceSynthesizeFactory
    {
        public string Name { get; } = "Haruka(SAPI)";

        public IVoiceSynthesize CreateVoiceSynthesizer() => new SapiVoiceSynthesizer();
    }

}
