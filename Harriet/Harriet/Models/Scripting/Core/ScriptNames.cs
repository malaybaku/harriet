
namespace Harriet.Models.Scripting
{
    //TODO: ここでリテラル文字列を大量に使ってるせいでIronPythonへの依存が強くなっている。
    //将来Roslyn Scriptingに移行したいなら直すこと

    /// <summary>スクリプト名の一覧を表します。</summary>
    static class ScriptNames
    {
        public const string Initialize = "initialize.py";
        public const string Start = "start.py";
        public const string Main = "main.py";
        public const string Close = "close.py";

    }
}
