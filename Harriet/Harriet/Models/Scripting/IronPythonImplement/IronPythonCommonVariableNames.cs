namespace Harriet.Models.Scripting
{
    /// <summary>IronPythonスクリプトで共有される変数名を定義します。</summary>
    public static class IronPythonCommonVariableNames
    {
        /// <summary>スクリプトへのAPIに対する名前です。</summary>
        public const string ApiVariableName = "harriet";
        /// <summary>アプリケーション中で共通のディクショナリに対する命名です</summary>
        public const string ApiGlobalDictionaryName = "harriet_globals";
    }
}
