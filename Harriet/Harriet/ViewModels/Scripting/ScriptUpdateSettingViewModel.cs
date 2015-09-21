using System.ComponentModel;
using Harriet.Models.Scripting;

namespace Harriet.ViewModels
{
    /// <summary>毎フレームごとに実行されるスクリプトの設定を表します。</summary>
    public class ScriptUpdateSettingViewModel : HarrietViewModelBase
    {
        public ScriptUpdateSettingViewModel(IScriptUpdateSetting setting)
        {
            UpdateEnabled = setting.UpdateEnabled;

            PropertyChanged += (_, __) => setting.UpdateEnabled = UpdateEnabled;
            PropertyChangedEventManager.AddHandler(
                setting,
                (_, __) => UpdateEnabled = setting.UpdateEnabled,
                nameof(UpdateEnabled)
                );
        }

        private bool _UpdateEnabled;
        /// <summary>毎フレームのアップデートが有効かを取得、設定します。</summary>
        public bool UpdateEnabled
        {
            get { return _UpdateEnabled; }
            set { SetAndRaisePropertyChanged(ref _UpdateEnabled, value); }
        }
    }
}
