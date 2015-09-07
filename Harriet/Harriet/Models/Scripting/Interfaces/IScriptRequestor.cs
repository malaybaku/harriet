using System;

namespace Harriet.Models.Scripting
{
    /// <summary>スクリプトの実行要求を投げるインターフェースを表します。</summary>
    public interface IScriptRequestor
    {
        /// <summary>スクリプトの実行を要求します。</summary>
        /// <param name="scriptName">実行したいスクリプト名</param>
        /// <param name="priority">実行の優先度</param>
        void Request(string scriptName, ScriptPriority priority);

        /// <summary>スクリプトの実行が要求されると発生します。</summary>
        event EventHandler<ScriptRequestorEventArgs> ScriptRequested;
    }

    /// <summary>IScriptRequestorの単純な実装です。</summary>
    public class SimpleScriptRequestor : IScriptRequestor
    {
        /// <summary>スクリプトの実行を要求します。</summary>
        /// <param name="scriptName">実行したいスクリプト名</param>
        /// <param name="priority">実行の優先度</param>
        public void Request(string scriptName, ScriptPriority priority = ScriptPriority.Normal)
        {
            ScriptRequested?.Invoke(this, new ScriptRequestorEventArgs(scriptName, priority));
        }

        /// <summary>スクリプトの実行が要求されると発生します。</summary>
        public event EventHandler<ScriptRequestorEventArgs> ScriptRequested;
    }

    /// <summary>スクリプトの実行要求内容をデータとして保持するイベント引数です。</summary>
    public class ScriptRequestorEventArgs : EventArgs
    {
        /// <summary>リクエスト内容でインスタンスを初期化します。</summary>
        /// <param name="scriptName">実行したいスクリプト名</param>
        /// <param name="priority">実行の優先度</param>
        public ScriptRequestorEventArgs(string scriptName, ScriptPriority priority=ScriptPriority.Normal)
        {
            ScriptName = scriptName;
            Priority = priority;
        }

        /// <summary>実行したいスクリプト名</summary>
        public string ScriptName { get; }
        /// <summary>実行の優先度</summary>
        public ScriptPriority Priority { get; }
    }
    
}
