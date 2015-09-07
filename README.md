Harriet 2015 Baku, 獏(ばく) 

Twitter: @baku_dreameater
E-mail: njslyr.trpg@gmail.com

NOTE: Documentation is written in Japanese. If English information needed, please contact to the author directly.

1. 概要
2. 想定する開発環境
3. ソリューション構成について
4. ライセンス

1. 概要
HarrietはWindows上で流麗に動作することを志向したデスクトップマスコットです。

利用者の方に対しては「可愛い」を主要なゴールにし、IronPythonスクリプトによって会話や自作キャラの表示に対応させているほか、プラグインによってキャラ表示のスキーム自体も拡張可能になっています。

一方、開発者向けに公開される本ソフトは、Windowsデスクトップ用のGUIフレームワークであるWPF(Windows Presentation Foundation)の応用例として「こんなことも出来ますよ」という例示です。少なくともWindows10以前でWindows上に非矩形(長方形でない)ウィンドウを表示させて自由に動かす事に関し、WPF以上のフレームワークは存在しません。狭い話題ではありますがデスクトップマスコットの実装は実は敷居が高くないことを知って貰うため、ソースコードを公開しています。

またXAMLはイラストやアニメーションの作成に有効であることを視覚的に伝えるのも本ソフトの目的です。


2. 想定する開発環境
製作者はVisual Studio 2015 Communityで開発しています。


3. ソリューション構成について
3-1. Harriet
本体です。

3-2. HarrietCharacterInterface
Harrietで読み込み可能なキャラの仕様をインターフェースとして定義しています。実装例としては3つほどあるので参考にしてください。

- DummyCharacterImplement : ただの球体をキャラ扱いさせる実装
- SimpleImageCharacter : 画像ファイルからキャラクターを生成
- SimpleXamlCharacter: XAMLファイルからキャラクターを生成

(※社畜ちゃん/プロ生ちゃんのキャラ実装についても別途公開予定です)


3-3. HarrietModelInterface
Harrietの内部処理に挿し込めるプラグインを作成するためのインターフェースを定義しています。現時点では「音声合成のために文章を前処理する」というのしか定義してませんが、今後増やす予定です。実装の例は現時点で一つだけです。

- MecabTextConverter : NMeCabライブラリを標準的な方法で用い、平文を発音に適したカタカナ表記に直す




4. ライセンス
"Lisence\lisence.txt"に記載しています。





5. 外部依存リソースとライセンス
規約上必須というわけではないんですが再利用性のためにライセンスに加えて著作元URLを付記しています。


5-1. IronPython
IronPythonはプログラミング言語Pythonの.NET実装であり、Apache License 2.0によって配布されています。
IronPython公式ページ: http://ironpython.net
Apache Licence 2.0(英語版): http://www.apache.org/licenses/LICENSE-2.0
Apache Licence 2.0(和訳版): http://sourceforge.jp/projects/opensource/wiki/licenses%2FApache_License_2.0


・Livet
LivetはWPF(Windows Presentation Foundation)の為のMVVM(Model-View-ViewModel)パターン実装用インフラストラクチャでありzlib/libpngライセンスで公開されています。
公式ページ: http://ugaya40.hateblo.jp/entry/livet
公式ページ(github): https://github.com/ugaya40/Livet


・MetroRadiance
MetroRadianceはWPFの外観変更用ライブラリでありMITライセンスで公開されています。
webページ: https://www.nuget.org/packages/MetroRadiance/



・AquesTalk
本ソフトは(株)アクエストの音声合成ライブラリAquesTalkを使用しており、著作権は同社に帰属します。これは具体的には頒布物のうち
・AqLicense.txt
・AquesTalk.dll
の2ファイルが含まれるフォルダとして本ソフトに含まれており、"dll\AquesTalk"以下に下記の8フォルダが存在します。
・f1
・f2
・m1
・m2
・r1
・imd1
・dvd
・jgr
これらのライセンスは上述の各フォルダに含まれる"AqLicense.txt"に従います。

蛇足として注記しておきますが、現在2015年6月時点で(株)アクエストが公開するAquesTalkには本節で掲載したのと異なるライセンスが適用されています。AquesTalkは世間一般に出回っているものでもライセンスのバリエーションが複数存在するので十分ライセンスを注意したうえでご利用下さい。
webページ: http://www.a-quest.com/


