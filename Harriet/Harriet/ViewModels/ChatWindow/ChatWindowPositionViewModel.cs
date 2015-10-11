using System.ComponentModel;

using Livet.Commands;

using Harriet.Models.Chat;

namespace Harriet.ViewModels
{
    public class ChatWindowPositionViewModel : HarrietViewModelBase
    {

        public ChatWindowPositionViewModel(ChatWindowPositionModel model)
        {
            _model = model;
            Layout = new ChatWindowLayoutViewModel(model.Layout);

            Left = model.Left;
            Top = model.Top;
            Width = model.Width;
            Height = model.Height;

            AssignToModel(model);
        }

        #region Left変更通知プロパティ
        private double _Left;
        /// <summary>ウィンドウ左端座標を取得、設定します。</summary>
        public double Left
        {
            get { return _Left; }
            set { UISetAndRaisePropertyChanged(ref _Left, value); }
        }
        #endregion

        #region Top変更通知プロパティ
        private double _Top;
        /// <summary>ウィンドウ上端座標を取得、設定します。</summary>
        public double Top
        {
            get { return _Top; }
            set { UISetAndRaisePropertyChanged(ref _Top, value); }
        }
        #endregion

        #region Height変更通知プロパティ
        private double _Height = 150;
        /// <summary>ウィンドウの高さを取得、設定します。</summary>
        public double Height
        {
            get { return _Height; }
            set { UISetAndRaisePropertyChanged(ref _Height, value); }
        }
        #endregion

        #region Width変更通知プロパティ
        private double _Width = 300;
        /// <summary>ウィンドウの幅を取得、設定します。</summary>
        public double Width
        {
            get { return _Width; }
            set { UISetAndRaisePropertyChanged(ref _Width, value); }
        }
        #endregion

        public ChatWindowLayoutViewModel Layout { get; }

        #region ResetSizeCommand
        private ViewModelCommand _resetSizeCommand;
        public ViewModelCommand ResetSizeCommand
            => _resetSizeCommand ?? (_resetSizeCommand = new ViewModelCommand(ResetSize));

        private void ResetSize() => _model.ResetSize();
        #endregion

        #region SetCurrentSizeToDefaultCommand
        private ViewModelCommand _setCurrentSizeToDefaultCommand;
        public ViewModelCommand SetCurrentSizeToDefaultCommand
            => _setCurrentSizeToDefaultCommand ?? (_setCurrentSizeToDefaultCommand = new ViewModelCommand(SetCurrentSizeToDefault));

        //NOTE: 整数値じゃないと見栄えが悪いので丸めておく
        private void SetCurrentSizeToDefault() => _model.SetDefaultSize((int)Width, (int)Height);
        #endregion


        private readonly ChatWindowPositionModel _model;

        private void AssignToModel(ChatWindowPositionModel model)
        {
            //OKなパターン: 明示的にメンバメソッドで実装
            PropertyChangedEventManager.AddHandler(
                model,
                OnModelPropertyChanged,
                string.Empty
                );

            this.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(Left))
                {
                    _model.Left = Left;
                }
                else if (e.PropertyName == nameof(Top))
                {
                    _model.Top = Top;
                }
                else if (e.PropertyName == nameof(Width))
                {
                    _model.Width = Width;
                }
                else if (e.PropertyName == nameof(Height))
                {
                    _model.Height = Height;
                }

            };

        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_model.Left))
            {
                Left = _model.Left;
            }
            else if (e.PropertyName == nameof(_model.Top))
            {
                Top = _model.Top;
            }
            else if (e.PropertyName == nameof(_model.Width))
            {
                Width = _model.Width;
            }
            else if (e.PropertyName == nameof(_model.Height))
            {
                Height = _model.Height;
            }
        }

    }
}
