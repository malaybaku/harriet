using System.ComponentModel;

namespace Harriet.Models.Scripting
{
    /// <summary>スクリプトのうち低速で回すものについて有効かどうかを表します。</summary>
    public interface IScriptRoutineSetting : INotifyPropertyChanged
    {
        /// <summary>キャラがロードされた直後、表示前に実行するスクリプトが有効かを取得します。</summary>
        bool InitEnabled { get; set; }

        /// <summary>キャラが表示された直後に実行するスクリプトが有効かを取得します。</summary>
        bool StartEnabled { get; set; }

        /// <summary>特に指示が無い限りループ実行されるスクリプトが有効かを取得します。</summary>
        bool MainEnabled { get; set; }

        /// <summary>キャラの表示終了時に実行するスクリプトが有効かを取得します。</summary>
        bool CloseEnabled { get; set; }

        /// <summary>パスを指定して実行するスクリプトが有効かを取得します。</summary>
        bool RequestEnabled { get; set; }

        /// <summary>Mainスクリプトの読み込みが終了してから次に読み込むまでの時間を取得します。</summary>
        double ScriptInterval { get; set; }
    }

    public class ScriptRoutineSetting : HarrietNotifiableModelBase, IScriptRoutineSetting
    {

        private bool _initEnabled;
        /// <summary>キャラがロードされた直後、表示前に実行するスクリプトが有効かを取得します。</summary>
        public bool InitEnabled
        {
            get { return _initEnabled; }
            set { SetAndRaisePropertyChanged(ref _initEnabled, value); }
        }

        private bool _startEnabled;
        /// <summary>キャラが表示された直後に実行するスクリプトが有効かを取得します。</summary>
        public bool StartEnabled
        {
            get { return _startEnabled; }
            set { SetAndRaisePropertyChanged(ref _startEnabled, value); }
        }

        private bool _mainEnabled;
        /// <summary>特に指示が無い限りループ実行されるスクリプトが有効かを取得します。</summary>
        public bool MainEnabled
        {
            get { return _mainEnabled; }
            set { SetAndRaisePropertyChanged(ref _mainEnabled, value); }
        }

        private bool _closeEnabled;
        /// <summary>キャラの表示終了時に実行するスクリプトが有効かを取得します。</summary>
        public bool CloseEnabled
        {
            get { return _closeEnabled; }
            set { SetAndRaisePropertyChanged(ref _closeEnabled, value); }
        }

        private bool _requestEnabled;
        /// <summary>パスを指定して実行するスクリプトが有効かを取得します。</summary>
        public bool RequestEnabled
        {
            get { return _requestEnabled; }
            set { SetAndRaisePropertyChanged(ref _requestEnabled, value); }
        }

        private double _scriptInterval = 10.0;
        /// <summary>Mainスクリプトの読み込みが終了してから次に読み込むまでの時間を取得します。</summary>
        public double ScriptInterval
        {
            get { return _scriptInterval; }
            set { SetAndRaisePropertyChanged(ref _scriptInterval, value); }
        }

    }

}
