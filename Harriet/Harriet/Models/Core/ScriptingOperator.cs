using System;
using System.Threading.Tasks;

using IronPython.Runtime;

using Harriet.CharacterInterface;

using Harriet.Models.BasicUtil;
using Harriet.Models.Chat;
using Harriet.Models.Voice;
using Harriet.Models.Scripting;
using Harriet.Models.Scripting.ScriptReaderStateMachine;

using Harriet.Views;
using Harriet.ViewModels;

namespace Harriet.Models.Core
{
    /// <summary>アプリケーションの起動から終了まででのスクリプト制御を行います。</summary>
    public class ScriptingOperator : IDisposable
    {
        public ScriptingOperator(
            string characterName,
            IMainWindow mainWindow,
            IHarrietCharacter character, 
            CharacterSetting setting)
        {
            _characterName = characterName;
            _mainWindow = mainWindow;
            _character = character;
            _setting = setting;

            Initialize();
        }

        /// <summary>フィールド初期化を行います。</summary>
        private void Initialize()
        {
            var chatWindowPosition = new ChatWindowPositionModel(
                _mainWindow,
                _setting.ChatWindowLayout,
                _relocator
                );

            _chatWindow = new ChatWindowModel(chatWindowPosition);

            var voiceOperator = new VoiceOperator(_setting.Voice);
            voiceOperator.LipSynchRequested += (_, e) => _lipSyncher = e.LipSyncher;

            var scriptRequestor = new SimpleScriptRequestor();

            var api = new ScriptApi(
                _mainWindow, _character, voiceOperator, _chatWindow, scriptRequestor,
                _setting,
                _setting.ScriptApi,
                _characterName
                );

            var dict = new PythonDictionary();
            _updateProcessor = new IronPythonUpdateProcessor(api, _setting.ScriptUpdate, dict);

            var ironPythonReader = new IronPythonMainScriptReader(api, dict);
            _scriptStateManager = new ScriptStateManager(ironPythonReader, _setting.ScriptRoutine, _characterName);

            scriptRequestor.ScriptRequested += (_, e) => _scriptStateManager.Request(
                e.ScriptName,
                e.Priority
                );

        }

        /// <summary>スクリプトのルーチンを開始します。</summary>
        public void Start()
        {
            if (IsStarted) return;
            IsStarted = true;

            var chatWindowViewModel = new ChatWindowViewModel(
                _chatWindow,
                _mainWindow,
                _setting.ShownCharacterName,
                _setting.ChatWindowText,
                _setting.ChatWindowColor
                );
            new ChatWindow { DataContext = chatWindowViewModel }.Show();

            var stateStart = new ReadInitialize(_scriptStateManager);
            var scriptStateMachine = new StateMachine(stateStart);
            Task.Run(() =>
            {
                while (!_scriptStateManager.IsDisposed)
                {
                    scriptStateMachine.Update();
                }
            });
        }

        /// <summary>ポーリングによってチャット枠の位置を直し、指定されたスクリプトを実行します。</summary>
        public void Update()
        {
            _relocator.Request();
            _updateProcessor.Update();
        }

        /// <summary>リソースを解放します。</summary>
        public void Dispose()
        {
            _chatWindow.Dispose();
            _scriptStateManager.Dispose();
        }

        /// <summary>初期化が終了したときに発生します。</summary>
        public event EventHandler Initialized
        {
            add { _scriptStateManager.Initialized += value; }
            remove { _scriptStateManager.Initialized -= value; }
        }

        /// <summary>全てのスクリプト読み込みが終了したときに発生します。</summary>
        public event EventHandler Closed
        {
            add { _scriptStateManager.Closed += value; }
            remove { _scriptStateManager.Closed -= value; }
        }

        /// <summary>口パクの値を取得します。</summary>
        public int LipSynchValue => _lipSyncher?.CurrentValue ?? 0;

        public bool IsStarted { get; private set; }

        public void Request(string scriptName, ScriptPriority priority) 
            => _scriptStateManager.Request(scriptName, priority);

        #region フィールド
        //上から降ってくるモデル
        private readonly string _characterName;
        private readonly CharacterSetting _setting;
        private readonly IHarrietCharacter _character;
        private readonly IMainWindow _mainWindow;

        //子要素として持つモデル
        private IronPythonUpdateProcessor _updateProcessor;
        private ScriptStateManager _scriptStateManager;
        //NOTE: チャット枠は操作としては完全にScriptApiの配下であることに注意しよう！
        private IChatWindowModel _chatWindow;

        /// <summary>チャット枠の位置直しに使う</summary>
        private readonly ChatWindowRelocateRequestor _relocator = new ChatWindowRelocateRequestor();

        private LipSyncher _lipSyncher;

        #endregion

    }
}
