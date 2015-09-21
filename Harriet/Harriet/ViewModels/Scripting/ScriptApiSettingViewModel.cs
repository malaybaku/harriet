using System.ComponentModel;
using Harriet.Models.Scripting;

namespace Harriet.ViewModels
{
    /// <summary>スクリプトAPIの設定を表します。</summary>
    public class ScriptApiSettingViewModel : HarrietViewModelBase
    {
        public ScriptApiSettingViewModel(IScriptApiSetting setting)
        {
            SerihuInterval = setting.SerihuInterval;
            PropertyChanged += (_, __) => setting.SerihuInterval = SerihuInterval;
            PropertyChangedEventManager.AddHandler(
                setting,
                (_, __) => SerihuInterval = setting.SerihuInterval,
                nameof(SerihuInterval)
                );
        }

        private double _SerihuInterval = 1.0;
        /// <summary>セリフの合間(発声があった場合、発声後にWaitする時間)の長さを取得、設定します。</summary>
        public double SerihuInterval
        {
            get { return _SerihuInterval; }
            set { SetAndRaisePropertyChanged(ref _SerihuInterval, value); }
        }

    }
}
