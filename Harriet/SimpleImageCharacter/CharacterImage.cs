using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using System.ComponentModel.Composition;

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Harriet.CharacterInterface;

namespace SimpleImageCharacter
{
    /// <summary>
    /// 画像読み込みによって生成されるキャラクターを表します。
    /// 注意として、ここで表示する画像はすべて同じサイズである必要があります。
    /// </summary>
    [Export(typeof(IHarrietCharacter))]
    public class SimpleImageCharacter : IHarrietCharacter
    {
        /// <summary>画像のソースとなるディレクトリを指定してキャラを初期化します。</summary>
        /// <param name="imageFileDirectory">画像ファイルが入ってるディレクトリ</param>
        public SimpleImageCharacter()
        {
            var dllPath = Assembly.GetExecutingAssembly().Location;
            //フォルダをさかのぼり、隣の"images"フォルダを見に行く
            var imagePath = Path.Combine(
                Path.GetDirectoryName(Path.GetDirectoryName(dllPath)), 
                "images"
                );

            _images = Directory.GetFiles(imagePath)
                .ToDictionary(
                    path => Path.GetFileNameWithoutExtension(path),
                    path => 
                    {
                        var img = new Image()
                        {
                            Source = new BitmapImage(new Uri(path)),
                            Visibility = Visibility.Collapsed
                        };
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                        return img;
                    });

            var grid = new Grid();
            foreach(var i in _images)
            {
                grid.Children.Add(i.Value);
            }
            _viewBox = new Viewbox()
            {
                Child = grid,
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new TransformGroup()
                {
                    Children = new TransformCollection
                    {
                        new TranslateTransform(),
                        new ScaleTransform(),
                        new RotateTransform()
                    }
                }
            };

            DefaultWidth = _images.First().Value.Source.Width;
            DefaultHeight = _images.First().Value.Source.Height;
            _viewBox.Width = DefaultWidth;
            _viewBox.Height = DefaultHeight;
        }

        //表示候補の画像を全部入れとく配列
        private readonly Dictionary<string, Image> _images;
        //実際の表示に使うコンテナ
        private readonly Viewbox _viewBox;

        public void Update()
        {
            //特に何もしない: 操作はIronPythonスクリプトに投げる
        }

        public double DefaultWidth { get; }
        public double DefaultHeight { get; }

        public double Width
        {
            get { return _viewBox.Dispatcher.Invoke(() => _viewBox.Width); }
            set { _viewBox.Dispatcher.Invoke(() => _viewBox.Width = value); }
        }
        public double Height
        {
            get { return _viewBox.Dispatcher.Invoke(() => _viewBox.Height); }
            set { _viewBox.Dispatcher.Invoke(() => _viewBox.Height = value); }
        }

        bool _isFirstDraw = true;
        public bool IsDrawNeeded => _isFirstDraw;

        /// <summary>パブリックプロパティにすることでIronPythonからのアクセスを許可</summary>
        public Direction Direction { get; set; }

        public Task<object> Draw()
        {
            return Task.Run(() =>
            {
                _isFirstDraw = false;
                return (object)_viewBox;
            });
        }

        /// <summary>パブリックプロパティにすることでIronPythonからのアクセスを許可</summary>
        public int LipSynchValue { get; set; }

        public void Dispose()
        {
            //ImageはDisposeしないでも大して問題無いらしい。ホントかな…？
        }

        /// <summary>
        /// 画像をファイル名で指定し、その画像を可視にするか不可視にするかを選ぶ
        /// </summary>
        /// <param name="key">画像のファイル名(拡張子は無し)</param>
        /// <param name="isVisible">trueなら可視、falseなら隠す</param>
        public void SetVisibility(string key, bool isVisible)
        {
            if (!_images.ContainsKey(key)) return;

            _images[key].Dispatcher.Invoke(() =>
            {
                _images[key].Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        /// <summary>指定した画像だけを可視にし、ほかを隠す</summary>
        /// <param name="key">可視にしたい画像のファイル名(拡張子は無し)</param>
        public void SetOnlyVisible(string key)
        {
            foreach(var kv in _images)
            {
                kv.Value.Dispatcher.Invoke(() =>
                {
                    kv.Value.Visibility = kv.Key == key ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }

        /// <summary>キャラのエフェクト(ぐにゃぐにゃするとか)がつけたいときの為に公開しておく。</summary>
        public Transform Transform => _viewBox.RenderTransform;

    }
}
