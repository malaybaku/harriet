using System;
using System.ComponentModel;
using System.Collections.Generic;

using Harriet.Models.Scripting;

namespace Harriet.ViewModels
{
    /// <summary>スクリプトの主処理部の設定を表します。</summary>
    public class ScriptRoutineSettingViewModel : HarrietViewModelBase
    {
        /// <summary>設定内容を用いてビューモデルを初期化します。</summary>
        /// <param name="setting">モデルで設定されたスクリプトの実行設定</param>
        public ScriptRoutineSettingViewModel(IScriptRoutineSetting setting)
        {
            InitEnabled = setting.InitEnabled;
            StartEnabled = setting.StartEnabled;
            MainEnabled = setting.MainEnabled;
            CloseEnabled = setting.CloseEnabled;
            RequestEnabled = setting.RequestEnabled;
            ScriptInterval = setting.ScriptInterval;

            AssignToSetting(setting);
        }

        #region InitEnabled変更通知プロパティ
        private bool _InitEnabled;
        /// <summary>初期化スクリプトが有効かを取得、設定します。</summary>
        public bool InitEnabled
        {
            get { return _InitEnabled; }
            set { SetAndRaisePropertyChanged(ref _InitEnabled, value); }
        }
        #endregion

        #region StartEnabled変更通知プロパティ
        private bool _StartEnabled;
        /// <summary>スタート時実行スクリプトが有効かを取得、設定します。</summary>
        public bool StartEnabled
        {
            get { return _StartEnabled; }
            set { SetAndRaisePropertyChanged(ref _StartEnabled, value); }
        }
        #endregion

        #region MainEnabled変更通知プロパティ
        private bool _MainEnabled;
        /// <summary>メインルーチンスクリプトが有効かを取得、設定します。</summary>
        public bool MainEnabled
        {
            get { return _MainEnabled; }
            set { SetAndRaisePropertyChanged(ref _MainEnabled, value); }
        }
        #endregion

        #region CloseEnabled変更通知プロパティ
        private bool _CloseEnabled;
        /// <summary>終了時スクリプトが有効かを取得、設定します。</summary>
        public bool CloseEnabled
        {
            get { return _CloseEnabled; }
            set { SetAndRaisePropertyChanged(ref _CloseEnabled, value); }
        }
        #endregion

        #region RequestEnabled変更通知プロパティ
        private bool _RequestEnabled;
        /// <summary>上記以外の自作スクリプト実行が有効かを取得、設定します。</summary>
        public bool RequestEnabled
        {
            get { return _RequestEnabled; }
            set { SetAndRaisePropertyChanged(ref _RequestEnabled, value); }
        }
        #endregion

        #region ScriptInterval変更通知プロパティ
        private double _ScriptInterval = 10.0;
        /// <summary>スクリプト間の実行間隔を取得、設定します。</summary>
        public double ScriptInterval
        {
            get { return _ScriptInterval; }
            set { SetAndRaisePropertyChanged(ref _ScriptInterval, value); }
        }
        #endregion

        private void AssignToSetting(IScriptRoutineSetting setting)
        {
            //NOTE: プロパティの種類が多いときはDictionary使う方が若干見栄えがよく、
            //リフレクではないのでスピードもそこそこ出る(ハズ)
            var reactions = new Dictionary<string, Action>()
            {
                { nameof(InitEnabled) , () => setting.InitEnabled = InitEnabled },
                { nameof(StartEnabled), () => setting.StartEnabled = StartEnabled },
                { nameof(MainEnabled), () => setting.MainEnabled = MainEnabled },
                { nameof(CloseEnabled), () => setting.CloseEnabled = CloseEnabled },
                { nameof(RequestEnabled), () => setting.RequestEnabled = RequestEnabled },
                { nameof(ScriptInterval), () => setting.ScriptInterval = ScriptInterval }
            };
            PropertyChanged += (_, e) => reactions[e.PropertyName]();
            PropertyChangedEventManager.AddHandler(setting, OnModelPropertyChanged, string.Empty);
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is IScriptRoutineSetting)) return;

            var setting = sender as IScriptRoutineSetting;

            var reactions = new Dictionary<string, Action>()
            {
                { nameof(InitEnabled) , () => InitEnabled = setting.InitEnabled },
                { nameof(StartEnabled), () => StartEnabled = setting.StartEnabled },
                { nameof(MainEnabled), () => MainEnabled = setting.MainEnabled },
                { nameof(CloseEnabled), () => CloseEnabled = setting.CloseEnabled },
                { nameof(RequestEnabled), () => RequestEnabled = setting.RequestEnabled },
                { nameof(ScriptInterval), () => ScriptInterval = setting.ScriptInterval }
            };

            reactions[e.PropertyName]();

        }

    }

}
