using Livet.Commands;
using Livet.Messaging.Windows;

using Harriet.Models.Core;

namespace Harriet.ViewModels
{
    /// <summary>設定ウィンドウのビューモデルです。</summary>
    public class SettingWindowViewModel : HarrietViewModelBase
    {

        /// <summary>モデルの設定一覧から設定画面のビューモデルを初期化します。</summary>
        /// <param name="modelSetting">モデル側の設定</param>
        public SettingWindowViewModel(CharacterSetting setting)
        {
            Voice = new VoiceViewModel(setting.Voice);

            ChatWindowText = new ChatWindowTextViewModel(setting.ChatWindowText);
            ChatWindowColor = new ChatWindowColorViewModel(setting.ChatWindowColor);
            ChatWindowLayout = new ChatWindowLayoutViewModel(setting.ChatWindowLayout);

            ShownCharacterName = new ShownCharacterNameViewModel(setting.ShownCharacterName);
            CharacterAppearance = new CharacterAppearanceViewModel(setting.CharacterAppearance);

            ScriptApi = new ScriptApiSettingViewModel(setting.ScriptApi);
            ScriptUpdate = new ScriptUpdateSettingViewModel(setting.ScriptUpdate);
            ScriptRoutine = new ScriptRoutineSettingViewModel(setting.ScriptRoutine);

            Nadenade = new NadeSettingViewModel(setting.Nadenade);

            CharacterName = setting.CharacterName;
        }

        #region CloseCommand
        private ViewModelCommand _CloseCommand;
        /// <summary>設定ウィンドウを閉じるコマンドを取得します。</summary>
        public ViewModelCommand CloseCommand
            => _CloseCommand ?? (_CloseCommand = new ViewModelCommand(Close));

        /// <summary> ビューを閉じます。 </summary>
        public void Close() => Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
        #endregion

        /// <summary>このビューモデルが扱っているキャラの名前(フォルダ名)を取得、設定します。</summary>
        public string CharacterName { get; }

        #region 子ビューモデル

        /// <summary>外部リンクの処理を取得します。</summary>
        public ExternalLinkViewModel ExternalLink { get; } = new ExternalLinkViewModel();

        /// <summary>音声の設定を取得します。</summary>
        public VoiceViewModel Voice { get; }

        /// <summary>チャット枠のフォント設定を取得します。</summary>        
        public ChatWindowTextViewModel ChatWindowText { get; }

        /// <summary>チャット枠の色設定を取得します。</summary>
        public ChatWindowColorViewModel ChatWindowColor { get; }

        /// <summary>チャット枠の配置設定を取得します。</summary>
        public ChatWindowLayoutViewModel ChatWindowLayout { get; }

        /// <summary>キャラクターの表示名設定を取得します。</summary>
        public ShownCharacterNameViewModel ShownCharacterName { get; }

        /// <summary>キャラクターの外観設定を取得します。</summary>
        public CharacterAppearanceViewModel CharacterAppearance { get; }

        /// <summary>スクリプトの有効化設定を取得します。</summary>
        public ScriptRoutineSettingViewModel ScriptRoutine { get; }

        /// <summary>毎フレーム実行されるスクリプトの設定を取得します。</summary>
        public ScriptUpdateSettingViewModel ScriptUpdate { get; }

        /// <summary>API固有の設定を取得します。</summary>
        public ScriptApiSettingViewModel ScriptApi { get; }

        /// <summary>撫で関連の設定を取得します。</summary>        
        public NadeSettingViewModel Nadenade { get; }

        #endregion

        private bool _allScriptEnabled = false;
        /// <summary>全スクリプトの有効/無効を取得、設定します。</summary>
        public bool AllScriptEnabled
        {
            get { return _allScriptEnabled; }
            set
            {
                if (_allScriptEnabled == value) return;
                _allScriptEnabled = value;
                //TODO: 本例ではわざわざVMから叩いてるが、本来こういう処理はXAMLで完結させた方がいいと思う
                ScriptRoutine.InitEnabled = value;
                ScriptRoutine.StartEnabled = value;
                ScriptRoutine.MainEnabled = value;
                ScriptRoutine.CloseEnabled = value;
                ScriptRoutine.RequestEnabled = value;
                ScriptUpdate.UpdateEnabled = value;
                RaisePropertyChanged();
            }
        }

    }
}
