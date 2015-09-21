using System.ComponentModel;

namespace Harriet.Models.Scripting
{
    /// <summary>毎フレームで実行されるスクリプトの設定を定義します。</summary>
    public interface IScriptUpdateSetting : INotifyPropertyChanged
    {
        /// <summary>アップデート処理が有効かどうかを取得、設定します。</summary>
        bool UpdateEnabled { get; set; }
    }

    /// <summary>毎フレームで実行されるスクリプトの設定を表します。</summary>
    public class ScriptUpdateSetting : HarrietNotifiableModelBase, IScriptUpdateSetting
    {
        private bool _updateEnabled;
        /// <summary>アップデート処理が有効かどうかを取得、設定します。</summary>
        public bool UpdateEnabled
        {
            get { return _updateEnabled; }
            set { SetAndRaisePropertyChanged(ref _updateEnabled, value); }
        }
    }


}
