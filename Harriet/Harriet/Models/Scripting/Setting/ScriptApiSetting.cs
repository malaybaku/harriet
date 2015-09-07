using System.ComponentModel;

namespace Harriet.Models.Scripting
{
    /// <summary>スクリプトAPIに特有の設定事項を定義します。</summary>
    public interface IScriptApiSetting : INotifyPropertyChanged
    {
        /// <summary>発声を行ったあとに間をあける時間を秒単位で取得、設定します。</summary>
        double SerihuInterval { get; set; }
    }

    /// <summary>スクリプトAPIに特有の設定事項を表します。</summary>
    public class ScriptApiSetting : HarrietNotifiableModelBase, IScriptApiSetting
    {
        private double _serihuInterval = 1.0;
        /// <summary>発声を行ったあとに間をあける時間を秒単位で取得、設定します。</summary>
        public double SerihuInterval
        {
            get { return _serihuInterval; }
            set { SetAndRaisePropertyChanged(ref _serihuInterval, value); }
        }
    }

}
