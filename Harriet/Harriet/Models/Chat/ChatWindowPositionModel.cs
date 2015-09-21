using Harriet.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harriet.Models.Chat
{
    /// <summary>チャット枠の位置、サイズ情報を表します。</summary>
    public class ChatWindowPositionModel : HarrietNotifiableModelBase
    {
        public ChatWindowPositionModel(
            IMainWindow mainWindow,
            IChatWindowLayoutSetting layout,
            IChatWindowRelocateRequestable relocateRequestor)
        {
            _mainWindow = mainWindow;
            Layout = layout;

            ResetSize();

            Left = 100;
            Top = 100;

            relocateRequestor.RelocateRequested += OnRelocateRequested;
        }


        #region Left変更通知プロパティ
        private double _Left;
        /// <summary>ウィンドウ左端座標を取得、設定します。</summary>
        public double Left
        {
            get { return _Left; }
            set { SetAndRaisePropertyChanged(ref _Left, value); }
        }
        #endregion

        #region Top変更通知プロパティ
        private double _Top;
        /// <summary>ウィンドウ上端座標を取得、設定します。</summary>
        public double Top
        {
            get { return _Top; }
            set { SetAndRaisePropertyChanged(ref _Top, value); }
        }
        #endregion

        #region Height変更通知プロパティ
        private double _Height = 150;
        /// <summary>ウィンドウの高さを取得、設定します。</summary>
        public double Height
        {
            get { return _Height; }
            set { SetAndRaisePropertyChanged(ref _Height, value); }
        }
        #endregion

        #region Width変更通知プロパティ
        private double _Width = 300;
        /// <summary>ウィンドウの幅を取得、設定します。</summary>
        public double Width
        {
            get { return _Width; }
            set { SetAndRaisePropertyChanged(ref _Width, value); }
        }
        #endregion

        public void ResetSize()
        {
            Width = Layout.DefaultWidth;
            Height = Layout.DefaultHeight;
        }

        //直接的に関数呼び出しで位置合わせを行ってもOK
        public void Relocate()
        {
            OnRelocateRequested(this, EventArgs.Empty);
        }

        private readonly IMainWindow _mainWindow;
        public IChatWindowLayoutSetting Layout { get; }

        //モデルから位置合わせが要請されたときの処理
        private void OnRelocateRequested(object sender, EventArgs e)
        {
            double oLeft = _mainWindow.Left;
            double oTop = _mainWindow.Top;
            double oWidth = _mainWindow.Width;
            double oHeight = _mainWindow.Height;

            switch (Layout.RelativePosition)
            {
                case RelativePosition.None:
                    return;
                case RelativePosition.RightTop:
                    Left = oLeft + oWidth;
                    Top = oTop + oHeight * 0.5 - Height;
                    return;
                case RelativePosition.LeftTop:
                    Left = oLeft - Width;
                    Top = oTop + oHeight * 0.5 - Height;
                    return;
                case RelativePosition.RightBottom:
                    Left = oLeft + oWidth;
                    Top = oTop + oHeight * 0.5;
                    return;
                case RelativePosition.LeftBottom:
                    Left = oLeft - Width;
                    Top = oTop + oHeight * 0.5;
                    return;
                default:
                    return;
            }
        }
    }
}
