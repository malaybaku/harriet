using Harriet.Models.Core;
using System;
using System.IO;

namespace Harriet.Models.Scripting
{
    /// <summary>ScriptStateMangerの基本的な実装を表します。</summary>
    public class ScriptStateManager : IScriptStateManager
    {
        /// <summary>設定オブジェクトとスクリプトの実行担当を受け取ってインスタンスを初期化します。</summary>
        /// <param name="reader">スクリプトが実行できるオブジェクト</param>
        /// <param name="setting">スクリプトの実行設定</param>
        /// <param name="characterName">キャラ名</param>
        public ScriptStateManager(IScriptReader reader, IScriptRoutineSetting setting, string characterName)
        {
            ScriptReader = reader;
            ScriptRoutineSetting = setting;
            _characterName = characterName;
        }

        #region インターフェース実装

        /// <summary>スクリプトの実行設定を取得します。</summary>
        public IScriptRoutineSetting ScriptRoutineSetting { get; }

        /// <summary>スクリプトを実際に処理するインスタンスを取得します。</summary>
        public IScriptReader ScriptReader { get; }

        /// <summary>現在実行要求があるスクリプトが存在するかを取得します。</summary>
        public bool IsScriptRequested { get; private set; }
        /// <summary>もっとも最近にリクエストされたスクリプトの名前を取得します。</summary>
        public string RequestedScriptName { get; private set; } = String.Empty;
        /// <summary>もっとも最近にリクエストされたスクリプトの実行優先度を取得します。</summary>
        public ScriptPriority RequestedScriptPriority { get; private set; } = ScriptPriority.Idle;

        /// <summary>ステートマシンが破棄済みであるかを取得します。</summary>
        public bool IsDisposed { get; private set; }

        /// <summary>基本スクリプト以外の読み込み要求が行われると発生します。</summary>
        public event EventHandler ScriptRequested;

        /// <summary>初期化スクリプトを読み終えると発生します。</summary>
        public event EventHandler Initialized;

        /// <summary>ステートマシンが終了すると発生します。</summary>
        public event EventHandler Closed;

        /// <summary>スクリプトの実行を要求します。</summary>
        /// <param name="scriptName">実行してほしいスクリプトの名前</param>
        /// <param name="requestPriority">実行の優先度</param>
        public void Request(string scriptName, ScriptPriority requestPriority)
        {
			if(requestPriority > _currentPriority)
            {
                RequestedScriptName = scriptName;
                RequestedScriptPriority = requestPriority;
                IsScriptRequested = true;
                ScriptReader.CancelRead();
                ScriptRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>リクエストを受けて処理を実行したことをインターフェースに通知します。</summary>
        public void NotifyRequestAccepted() => IsScriptRequested = false;

        /// <summary>ステートが初期化スクリプトを読み終えた事を通知します。</summary>
        public void NotifyInitialized() => Initialized?.Invoke(this, EventArgs.Empty);

        /// <summary>ブロッキングしてスクリプトを実行します。</summary>
        /// <param name="scriptName">実行するスクリプト名</param>
        /// <param name="priority">スクリプトの優先度</param>
        public void Read(string scriptName, ScriptPriority priority)
        {
            try
            {
                _currentPriority = priority;
                ScriptReader.ReadAsync(GetScriptPath(scriptName)).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is OperationCanceledException)
                {

                }
            }
            finally
            {
                //待ち状態にしとかないとリクエストが通らない事に注意
                _currentPriority = ScriptPriority.Idle;
            }
        }

        /// <summary>スクリプトの読み込みをやめ、リソースを解放します。</summary>
        public void Dispose()
        {
            ScriptReader.Dispose();
            IsDisposed = true;
            Closed?.Invoke(this, EventArgs.Empty);
        }


        #endregion

        #region プライベート実装

        private ScriptPriority _currentPriority = ScriptPriority.DoNotExecute;

        private readonly string _characterName;

        /// <summary>ファイル名にディレクトリ位置を付与し、パスとしてスクリプトを指定できる文字列を取得します。</summary>
        /// <param name="scriptName">スクリプト名("main.py"など)</param>
        /// <returns>実行ファイルから見たスクリプトへのパス</returns>
        private string GetScriptPath(string scriptName)
        {
            return Path.Combine(
                DirectoryNames.GetCharacterScriptDirectory(this._characterName),
                scriptName
                );
        }

        #endregion

    }
}
