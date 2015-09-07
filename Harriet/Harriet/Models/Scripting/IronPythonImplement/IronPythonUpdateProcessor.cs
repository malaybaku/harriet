using System;
using System.IO;
using System.Windows;

using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

using Harriet.Models.Core;

namespace Harriet.Models.Scripting
{
    class IronPythonUpdateProcessor
    {
        /// <summary> 毎回のアップデートで呼び出され実行されるスクリプトの名前 </summary>
        private const string UpdateScriptName = "update.py";

        /// <summary>APIの設定を用いてインスタンスを初期化します。 </summary>
        /// <param name="api">IronPython用APIの何らかの実装</param>
        /// <param name="setting">更新処理の設定</param>
        /// <param name="dictionary">キャラに対応するグローバル変数用にディクショナリ</param>
        public IronPythonUpdateProcessor(IScriptApi api, IScriptUpdateSetting setting, PythonDictionary dictionary)
        {
            _setting = setting;

            var engine = Python.CreateEngine();

            //名前参照にexeのディレクトリとキャラのディレクトリを追加
            var paths = engine.GetSearchPaths();
            paths.Add(Environment.CurrentDirectory);
            paths.Add(DirectoryNames.GetCharacterScriptDirectory(api.CharacterName));
            engine.SetSearchPaths(paths);

            //可視領域を限界まで広く取るためビルトインスコープにぶち込んでおく
            ScriptScope builtin = engine.GetBuiltinModule();
            builtin.SetVariable(IronPythonCommonVariableNames.ApiVariableName, api);
            builtin.SetVariable(IronPythonCommonVariableNames.ApiGlobalDictionaryName, dictionary);

            string path = Path.Combine(DirectoryNames.GetCharacterScriptDirectory(api.CharacterName), UpdateScriptName);

            //高スピードで読むので先にコンパイル
            try
            {
                _updateCode = engine.CreateScriptSourceFromFile(path).Compile();
                IsValid = true;
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        /// <summary>アップデート処理が認可されているかを取得します。</summary>
        public bool IsEnabled => IsValid && _setting.UpdateEnabled;

        /// <summary>アップデート処理を行うスクリプトが正常にコンパイルできたかを取得します。</summary>
        public bool IsValid { get; private set; }

        /// <summary> 毎回のアップデートで呼び出される関数 </summary>
        public void Update()
        {
            if (!IsEnabled) return;

            try
            {
                _updateCode.Execute(_updateCode.Engine.Runtime.Globals);
            }
            catch (Exception ex)
            {
                //何か起きた瞬間に止まる感じ
                IsValid = false;
#if DEBUG
                MessageBox.Show($"error when updating:{ex.Message}", "harriet-debug error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            }
        }

        private readonly CompiledCode _updateCode;

        private readonly IScriptUpdateSetting _setting;

    }
}
