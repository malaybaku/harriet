using System;

namespace Harriet.Models.Scripting
{
    /// <summary>スクリプト読み込みの状態遷移をサポートする操作を定義します。</summary>
    public interface IScriptStateManager : IDisposable
    {
        /// <summary>スクリプトの読み込み設定を取得します。</summary>
        IScriptRoutineSetting ScriptRoutineSetting { get; }

        /// <summary>スクリプトを実際に処理するインスタンスを取得します。</summary>
        IScriptReader ScriptReader { get; }

        /// <summary>現在実行要求があるスクリプトが存在するかを取得します。</summary>
        bool IsScriptRequested { get; }
        /// <summary>もっとも最近にリクエストされたスクリプトの名前を取得します。</summary>
        string RequestedScriptName { get; }
        /// <summary>もっとも最近にリクエストされたスクリプトの実行優先度を取得します。</summary>
        ScriptPriority RequestedScriptPriority { get; }
        /// <summary>ステートマシンが破棄済みであるかを取得します。</summary>
        bool IsDisposed { get; }

        /// <summary>スクリプトの実行を要求します。</summary>
        /// <param name="scriptName">実行してほしいスクリプトの名前</param>
        /// <param name="requestPriority">実行の優先度</param>
        void Request(string scriptName, ScriptPriority requestPriority);

        /// <summary>リクエストを受けて処理を実行したことをインターフェースに通知します。</summary>
        void NotifyRequestAccepted();
        /// <summary>初期化処理が完了したことを通知します。</summary>
        void NotifyInitialized();

        /// <summary>指定したスクリプトを実行します。</summary>
        /// <param name="scriptName">
        /// スクリプト名。
        /// 想定入力は"main.py"などキャラのスクリプトフォルダに置かれたスクリプト名で、
        /// これを"characters\hoge\script\"みたいなディレクトリ名と連結するのはインターフェース実装側の責任
        /// </param>
        /// <param name="priority">スクリプトの実行優先度</param>
        void Read(string scriptName, ScriptPriority priority);
        /// <summary>基本スクリプト以外の読み込み要求が行われると発生します。</summary>
        event EventHandler ScriptRequested;
        /// <summary>初期化のステートを抜けると発生します。</summary>
        event EventHandler Initialized;
        /// <summary>ステートマシンが終了すると発生します。</summary>
        event EventHandler Closed;
    }

    /// <summary>スクリプトの実行優先順位を指定します。</summary>
    public enum ScriptPriority
    {
        /// <summary>スクリプトは実行されません。</summary>
        DoNotExecute = 0,
        /// <summary>スクリプト間の待ち状態の優先度です。スクリプトは実行されません。</summary>
        Idle = 1,
        /// <summary>スクリプトを読んでいない状態であればスクリプトが実行されます。</summary>
        WhenIdle = 3,
        /// <summary>Mainスクリプトと同じ優先度です。スクリプトを読んでいない状態であればスクリプトが実行されます。</summary>
        Main = 5,
        /// <summary>Mainスクリプトの読み込み中、またはスクリプトを読んでいない状態であればスクリプトが実行されます。</summary>
        Normal = 7,
        /// <summary>Normal以下の優先順位の状態であればスクリプトが実行されます。</summary>
        High = 9,
        /// <summary>スクリプトでは使ってはいけません。Close処理の優先度であり、最も高い優先度です</summary>
        Close = 10
    }
}
