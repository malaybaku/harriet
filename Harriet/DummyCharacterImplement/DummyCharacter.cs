using System.Threading.Tasks;
using System.ComponentModel.Composition;

using System.Windows.Media;
using System.Windows.Shapes;

using Harriet.CharacterInterface;

namespace DummyCharacterImplement
{

    /// <summary>キャラの最低限の実装例としてボールを表示</summary>
    [Export(typeof(IHarrietCharacter))]
    public class DummyCharacter : IHarrietCharacter
    {
        public DummyCharacter()
        {
            //NOTE: ロード処理は本体側のUIスレッドで行われる
            _ellipse = new Ellipse
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Red
            };
        }

        /// <summary>毎フレームごとのアップデート処理</summary>
        public void Update()
        {
            //作り方によってはココで瞬きとかを実装することも可能
            return;
        }

        public double DefaultWidth => 100;

        public double DefaultHeight => 100;

        /// <summary> 実際に表示する幅を取得、設定 </summary>
        public double Width
        {
            get { return _ellipse.Dispatcher.Invoke(() => _ellipse.Width); }
            set { _ellipse.Dispatcher.Invoke(() => _ellipse.Width = value); }
        }

        /// <summary> 実際に表示する高さを取得、設定 </summary>
        public double Height
        {
            get { return _ellipse.Dispatcher.Invoke(() => _ellipse.Height); }
            set { _ellipse.Dispatcher.Invoke(() => _ellipse.Height = value); }
        }

        private bool _isBeforeFirstDraw = true;
        /// <summary>割とありがちな実装パターン: 初回だけ描画対象を渡す</summary>
        public bool IsDrawNeeded => _isBeforeFirstDraw;

        public Direction Direction { get; set; }

        //Ellipseを表示物として渡す
        public Task<object> Draw()
        {
            return Task.Run(() =>
            {
                _isBeforeFirstDraw = false;
                return _ellipse as object;
            });
        }

        /// <summary>
        /// セッターはあるけど実際は何もしない。
        /// 本来は0(無言)~5(大き目の声)の段階に合わせた口パクアニメーションに反映させることが可能
        /// </summary>
        public int LipSynchValue
        {
            set { }
        }

        public void Dispose()
        {
            //何もしない
        }

        private Ellipse _ellipse;

    }

}
