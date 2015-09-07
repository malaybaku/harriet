using System.Threading.Tasks;

using System.Windows.Controls;
using System.Xaml;

using Harriet.CharacterInterface;


namespace SimpleXamlCharacter
{
    /// <summary>XAMLファイルから生成されるキャラを表します。</summary>
    public class XamlCharacter : IHarrietCharacter
    {
        public XamlCharacter(string xamlFilename)
        {
            Character = (UserControl)XamlServices.Load(xamlFilename);

            DefaultWidth = Character.Width;
            DefaultHeight = Character.Height;
        }

        public UserControl Character { get; }

        public void Update()
        {
            //何もしない
        }

        public double DefaultWidth { get; }
        public double DefaultHeight { get; }

        public double Width
        {
            get { return Character.Dispatcher.Invoke(() => Character.Width); }
            set { Character.Dispatcher.Invoke(() => { Character.Width = value; }); }
        }

        public double Height
        {
            get { return Character.Dispatcher.Invoke(() => Character.Height); }
            set { Character.Dispatcher.Invoke(() => { Character.Height = value; }); }
        }

        /// <summary>初回だけ描画する為のフラグ</summary>
        private bool _isFirstDraw = true;
        public bool IsDrawNeeded => _isFirstDraw;

        public Task<object> Draw()
        {
            return Task.Run(() =>
            {
                _isFirstDraw = false;
                return (object)Character;
            });
        }


        /// <summary>パブリックプロパティにすることでIronPythonからのアクセスを許可</summary>
        public Direction Direction { get; set; }
        /// <summary>パブリックプロパティにすることでIronPythonからのアクセスを許可</summary>
        public int LipSynchValue { get; set; }

        public void Dispose()
        {
            //UserControlはDisposeしないで良いので特になし。
        }
    }
}
