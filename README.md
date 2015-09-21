Harriet 2015 Baku, 獏(ばく) 

Twitter: @baku_dreameater
E-mail: njslyr.trpg@gmail.com

NOTE: Documentation is written in Japanese. If English information needed, please contact to the author directly.

1. 概要
2. 想定する開発環境
3. ソリューション構成について
4. ライセンス
5. 利用しているリソース


### 1. 概要
HarrietはWindows上で動作するデスクトップマスコットです。

IronPythonスクリプトによって会話や自作キャラの表示に対応させているほか、プラグインによってキャラ表示のスキーム自体も拡張可能になっています。

フレームワークとしてはWPFを用いています。


### 2. 想定する開発環境
製作者はWindows 10 / Visual Studio 2015 Communityで開発しています。


### 3. ソリューション構成について

#### 3-1. Harriet
本体です。

#### 3-2. HarrietCharacterInterface
Harrietで読み込み可能なキャラの仕様をインターフェースとして定義しています。実装例としては3つほどあるので参考にしてください。

- DummyCharacterImplement : ただの球体をキャラ扱いさせる実装
- SimpleImageCharacter : 画像ファイルからキャラクターを生成
- SimpleXamlCharacter: XAMLファイルからキャラクターを生成

(※社畜ちゃん/プロ生ちゃんのキャラ実装についても別途公開予定です)


#### 3-3. HarrietModelInterface
Harrietの内部処理に挿し込めるプラグインを作成するためのインターフェースを定義しています。現時点では「音声合成器の追加」「音声合成のために文章を前処理する」の２点を定義しており、実装例として下記のプロジェクトがあります。。

- SapiVoiceSynthesize: 音声合成器の実装です。WindowsのSpeech APIを使って読み上げ処理を行います。
- MecabTextConverter : 音声合成(AquesTalk)のための前処理実装です。形態素解析ライブラリであるNMeCabを用いて、平文を発音に適したカタカナ表記に直します(TestMecabTextConverterプロジェクトが簡単なテスト例になっています)。


### 4. ライセンス
"Lisence\lisence.txt"に記載しています。


### 5. 利用しているリソース
各リソースごとのライセンスは上述のライセンス中に示しています。

#### IronPython
プログラミング言語Pythonの.NET実装です。 http://ironpython.net

#### Livet
MVVM(Model-View-ViewModel)インフラストラクチャです。 http://ugaya40.hateblo.jp/entry/livet

#### MetroRadiance
WPFの外観変更用ライブラリです。 https://www.nuget.org/packages/MetroRadiance/

#### NAudio
音声データの処理ライブラリです。 https://naudio.codeplex.com/

### NMeCab
形態素解析エンジンです。 https://osdn.jp/projects/nmecab/

#### AquesTalk
音声合成ライブラリです。 http://www.a-quest.com/



