using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Windows;

using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using Harriet.Models.Core;

namespace Harriet.Models.Scripting
{
    /// <summary>IronPythonで処理時間の長いスクリプトを読み込むプロセッサを表します。</summary>
    internal class IronPythonMainScriptReader : IScriptReader
    {
        /// <summary> APIの設定を用いてスクリプト読み込み器を初期化します。 </summary>
        /// <param name="api"></param>
        /// <param name="dictionary">スクリプトのグローバル変数を入れておくディクショナリ</param>
        public IronPythonMainScriptReader(IScriptApi api, PythonDictionary dictionary)
        {
            InitializeEngine(api, dictionary);
        }

        #region インターフェース実装

        /// <summary>現在スクリプトを実行中かを取得します。</summary>
        public bool IsExecutingScript
        {
            get
            {
                lock(_isExecutingScriptLock) return _isExecutingScript;
            }
            private set
            {
                lock(_isExecutingScriptLock) _isExecutingScript = value;
            }
        }

        /// <summary>キャンセル可能なスクリプト読み込みを開始します。</summary>
        /// <param name="filename">スクリプトのパス</param>
        public Task ReadAsync(string filename)
        {
            if (!File.Exists(filename)) return Task.Run(() => { });

            if (IsExecutingScript) return Task.Run(() => { });
            IsExecutingScript = true;

            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var threadScript = new Thread(() => ExecuteFile(filename));

            EventHandler onCancelRequested = (_, __) => cts.Cancel();
            this.CancelReadRequesed += onCancelRequested;

            return Task.Run(async () =>
            {
                threadScript.Start();
                await Task.Delay(Timeout.Infinite, token);
            }, token).ContinueWith(t =>
            {
                CancelReadRequesed -= onCancelRequested;
                if (threadScript != null && threadScript.IsAlive)
                {
                    threadScript.Abort();
                }
                IsExecutingScript = false;
            });                
        }

        /// <summary>スクリプトの読み込みをキャンセルします。</summary>
        public void CancelRead() => CancelReadRequesed?.Invoke(this, EventArgs.Empty);

        /// <summary>スクリプト読み込みを中止し、リソースを解放します。</summary>
        public void Dispose() => CancelRead();

        #endregion

        #region private

        private bool _isExecutingScript = false;
        object _isExecutingScriptLock = new object();

        /// <summary> 読み込み終了までが長いスクリプトを実行するエンジン </summary>
        private ScriptEngine _ironPythonEngine;

        /// <summary>スクリプト読み込みのキャンセルが要求されると発生します。</summary>
        private event EventHandler CancelReadRequesed;

        /// <summary> スクリプティングAPIの設定にもとづいて初期化を行う </summary>
        /// <param name="api">APIとなるインスタンス</param>
        /// <param name="globals">グローバル変数</param>
        private void InitializeEngine(IScriptApi api, PythonDictionary globals)
        {
            _ironPythonEngine = Python.CreateEngine();
            //名前参照にexeのディレクトリとキャラのディレクトリを追加
            var paths = _ironPythonEngine.GetSearchPaths();
            paths.Add(Environment.CurrentDirectory);
            paths.Add(DirectoryNames.GetCharacterScriptDirectory(api.CharacterName));
            _ironPythonEngine.SetSearchPaths(paths);

            //可視領域を限界まで広く取るためビルトインスコープにぶち込んでおく
            ScriptScope builtin = _ironPythonEngine.GetBuiltinModule();
            builtin.SetVariable(IronPythonCommonVariableNames.ApiVariableName, api);
            builtin.SetVariable(IronPythonCommonVariableNames.ApiGlobalDictionaryName, globals);
        }

        /// <summary> ブロッキング実行でスクリプトを実行する </summary>
        /// <param name="filename"> スクリプトのパス </param>
        private void ExecuteFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            try
            {
                _ironPythonEngine.ExecuteFile(filename, _ironPythonEngine.Runtime.Globals);
                CancelRead();
            }
            catch (ThreadAbortException)
            {
                //他からキャンセル要求が飛んで来たケース: 想定通りなので無視
                return;
            }
            catch (SyntaxErrorException ex)
            {
                ProcessScriptSyntaxError(ex);
                CancelRead();
            }
            catch (Exception ex)
            {
                //面倒なのでその他有象無象は一様にキャッチ
                ProcessScriptExceptionAndExit(ex);
                CancelRead();
            }
        }

        /// <summary> スクリプト実行中のエラーを握りつぶす </summary>
        /// <param name="ex">  </param>
        /// <param name="additiveInformation">  </param>
        private void ProcessScriptExceptionAndExit(Exception ex, string additiveInformation = "")
        {
            //スレッド中断の場合想定通りなので黙殺
            if (ex is ThreadAbortException)
            {
                return;
            }

            string exMsg = ex.Message;
            if (ex is AggregateException)
            {
                var aex = ex as AggregateException;
                exMsg = String.Join(
                    ",", 
                    aex.InnerExceptions.Select(e => e.Message)
                    );
            }

            MessageBox.Show(
                string.Format(
                    "IronPythonスクリプト実行中にエラーが発生しました: "
                     + " exception type: {0}, message: {1}",
                        ex.GetType().ToString(),
                        exMsg
                        )
                     + additiveInformation,
                "Harriet: Pythonエラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
        }

        /// <summary> シンタックスエラーを処理 </summary>
        /// <param name="ex"></param>
        private void ProcessScriptSyntaxError(SyntaxErrorException ex)
        {
            ProcessScriptExceptionAndExit(
                ex,
                string.Format("\n ファイル名:\"{0}\" \n 位置: 第{1}行, 第{2}列",
                    ex.SourcePath,
                    ex.Line,
                    ex.Column
                    )
                );
        }

        #endregion
    }
}
