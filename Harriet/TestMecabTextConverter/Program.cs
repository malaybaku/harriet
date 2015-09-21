using System;

namespace TestMecabTextConverter
{
    /// <summary>NMeCabの変換機能をチェックするためのクラスです。</summary>
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new MecabTextConverter.MeCabTextConverterFactory();
            var converter = factory.CreateConverter();

            var res = converter.Convert("今日は、激しく雨が降るらしいですよ？傘を持ってないとずぶ濡れになってしまいます。");
            Console.WriteLine(res);
        }
    }
}
