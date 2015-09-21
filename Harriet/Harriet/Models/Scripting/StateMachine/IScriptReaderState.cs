using System;
using System.Threading.Tasks;

using Harriet.Models.BasicUtil;
using System.Threading;

namespace Harriet.Models.Scripting
{
    namespace ScriptReaderStateMachine
    {
        /// <summary>スクリプトの読み込み手順を表すステートマシンのステート実装を表します。</summary>
        abstract internal class IScriptStateManagerStateBase : IState
        {
            public IScriptStateManagerStateBase(IScriptStateManager manager)
            {
                StateManager = manager;
            }
            
            protected IScriptStateManager StateManager { get; }

            public virtual void Enter()
            {
            }

            public virtual IState Update()
            {
                return this;
            }

            public virtual void Quit()
            {
            }
        }

        /// <summary>初期化スクリプト(キャラの表示開始前にUIスレッドで実行される)を読み込む状態を表します。</summary>
        internal class ReadInitialize : IScriptStateManagerStateBase
        {
            public ReadInitialize(IScriptStateManager manager) : base(manager) { }

            public override IState Update()
            {
                if (StateManager.ScriptRoutineSetting.InitEnabled)
                {
                    StateManager.Read(ScriptNames.Initialize, ScriptPriority.High);
                }

                return new ReadStart(StateManager);
            }

            public override void Quit()
            {
                base.Quit();
                StateManager.NotifyInitialized();
            }
        }

        /// <summary>開始スクリプト(キャラの表示直後に1度だけ実行される)を読み込む状態を表します。</summary>
        internal class ReadStart : IScriptStateManagerStateBase
        {
            public ReadStart(IScriptStateManager manager) : base(manager) { }

            public override IState Update()
            {
                if (!StateManager.ScriptRoutineSetting.StartEnabled)
                {
                    return new ReadMain(StateManager);
                }

                StateManager.Read(ScriptNames.Start, ScriptPriority.High);
                return new WaitInterScript(StateManager);
            }
        }

        /// <summary>メインスクリプト(一定間隔で呼び出される)を読み込む状態を表します。</summary>
        internal class ReadMain : IScriptStateManagerStateBase
        {
            public ReadMain(IScriptStateManager manager) : base(manager) { }

            public override IState Update()
            {

                if (StateManager.ScriptRoutineSetting.MainEnabled)
                {
                    StateManager.Read(ScriptNames.Main, ScriptPriority.Main);
                }
                return new WaitInterScript(StateManager);
            }
        }

        /// <summary>スクリプトを読まず待機する状態を表します。</summary>
        internal class WaitInterScript : IScriptStateManagerStateBase
        {
            public WaitInterScript(IScriptStateManager manager) : base(manager) { }

            public override void Enter()
            {
                _cts = new CancellationTokenSource();
                _token = _cts.Token;
                _task = Task.Delay((int)(StateManager.ScriptRoutineSetting.ScriptInterval * 1000));

                StateManager.ScriptRequested += OnScriptRequested;
            }

            private Task _task;
            private CancellationTokenSource _cts;
            private CancellationToken _token;

            public override IState Update()
            {
                //リクエスト+キャンセルの組み合わせでココに飛んで来た場合はWaitしない
                if(!StateManager.IsScriptRequested)
                {
                    try
                    {
                        _task.Wait(_token);
                    }
                    catch(OperationCanceledException)
                    {
                        //単なるキャンセル発動なので握りつぶす
                    }
                }

                //特に何も無いならメインスクリプト
                if(!StateManager.IsScriptRequested)
                {
                    return new ReadMain(StateManager);
                }

                //ちょっとカッコ悪いけどリクエスト種類で場合分け
                if (StateManager.RequestedScriptName == ScriptNames.Close)
                {
                    return new ReadClose(StateManager);
                }
                else
                {
                    return new ReadRequestedScript(StateManager);
                }
            }

            public override void Quit()
            {
                //明示的に外すのが大人。
                StateManager.ScriptRequested -= OnScriptRequested;
            }

            private void OnScriptRequested(object sender, EventArgs e) => _cts?.Cancel();

        }

        /// <summary>ユーザから指示された任意のスクリプトを読み込む状態を表します。</summary>
        internal class ReadRequestedScript : IScriptStateManagerStateBase
        {
            public ReadRequestedScript(IScriptStateManager manager) : base(manager)
            {
            }

            public override IState Update()
            {
                //処理開始と同時に要求フラグを取り下げ、複数回の読み込みを防ぐ
                StateManager.NotifyRequestAccepted();
                if (StateManager.ScriptRoutineSetting.RequestEnabled)
                {
                    StateManager.Read(StateManager.RequestedScriptName, StateManager.RequestedScriptPriority);
                }
                return new WaitInterScript(StateManager);
            }
        }

        /// <summary>終了スクリプト(アプリケーション終了直前に実行される)を読み込む状態を表します。</summary>
        internal class ReadClose : IScriptStateManagerStateBase
        {
            public ReadClose(IScriptStateManager manager) : base(manager) { }

            public override IState Update()
            {
                if (StateManager.ScriptRoutineSetting.CloseEnabled)
                {
                    StateManager.Read(ScriptNames.Close, ScriptPriority.Close);
                }
                return new Closed(StateManager);
            }

        }

        /// <summary>終了状態を表します。</summary>
        internal class Closed : IScriptStateManagerStateBase
        {
            public Closed(IScriptStateManager manager) : base(manager) { }

            public override void Enter()
            {
                base.Enter();
                StateManager.Dispose();
            }

            public override IState Update()
            {
                //ココには到達しない: Enterした段階でプロセッサが死ぬ
                throw new ObjectDisposedException("IronPythonProcessor is already disposed");
            }
        }

    }

}
