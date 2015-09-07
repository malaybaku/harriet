using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Expression.Interactivity.Core;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.Windows;

using Harriet.Models;
using Harriet.Models.Core;

namespace Harriet.ViewModels
{
    //CAUTION: このクラスはVMとModelが兼任みたくなってることに注意(修正予定)

    /// <summary>キャラを表示するウィンドウを表します。</summary>
    public class MainWindowViewModel : HarrietViewModelBase, IMainWindow, ICharacterSelector, INadeable
    {
        #region コメント
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        #endregion

        /// <summary>ビューモデルを初期化します。</summary>
        public MainWindowViewModel()
        {
            if (!Directory.Exists(DirectoryNames.CharacterDirectory))
            {
                AvailableCharacters = new MenuItem[] { };
            }
            else
            {
                var displayedNames = DirectoryNames.GetAvailableDisplayNames();
                //CF: ココで初期化する以外のシナリオとしてはモデルから文字列リスト付きのイベントが投げられるパターンなどがあり得る。
                //TODO: Validなキャラだけ通すように直した方がいいとおもいまーす！
                AvailableCharacters = DirectoryNames.AvailableCharacters
                    .Select(n => new MenuItem
                    {
                        Header = displayedNames[n],
                        Command = new ActionCommand(() => OnCharacterSelected(n))
                    })
                    .ToArray();
            }

            Topmost = CommonSettingRecord.Load().Topmost;
        }

        /// <summary>ビュー側の初期化が終了した後で主処理を開始します。</summary>
        public void Initialize()
        {
            _modelCore = ModelCore.LoadAndStart(this, this, this);
        }

        #region インターフェース実装

        #region INadeable

        private double _Nadenade;
        /// <summary>撫でた量を取得、設定します。</summary>
        public double Nadenade
        {
            get { return _Nadenade; }
            set { SetAndRaisePropertyChanged(ref _Nadenade, value); }
        }

        #endregion

        #region ICharacterSelector

        /// <summary> キャラ変更のリクエストを通知する </summary>
        public event EventHandler<CharacterSelectedEventArgs> CharacterSelected;

        #endregion

        #region IMainWindow

        /// <summary>ウィンドウ中心のX座標を取得、設定します。</summary>
        public double CenterX
        {
            get { return Left + 0.5 * Width; }
            set
            {
                if (Math.Abs(Left + 0.5 * Width - value) < 1.0)
                    return;
                Left = value - 0.5 * Width;
                RaisePropertyChanged();
            }
        }

        /// <summary>ウィンドウ中心のY座標を取得、設定します。</summary>
        public double CenterY
        {
            get { return Top + 0.5 * Height; }
            set
            {
                if (Math.Abs(Top + 0.5 * Height - value) < 1.0)
                    return;
                Top = value - 0.5 * Height;
                RaisePropertyChanged();
            }
        }

        #region Width変更通知プロパティ
        private double _Width = 200.0;
        /// <summary>ウィンドウの幅を取得、設定します。</summary>
        public double Width
        {
            get { return _Width; }
            set { SetAndRaisePropertyChanged(ref _Width, value); }
        }
        #endregion

        #region Height変更通知プロパティ
        private double _Height = 200.0;
        /// <summary>ウィンドウの高さを取得、設定します。</summary>
        public double Height
        {
            get { return _Height; }
            set { SetAndRaisePropertyChanged(ref _Height, value); }
        }
        #endregion

        #region Left変更通知プロパティ
        private double _Left;
        /// <summary>ウィンドウ左端の座標を取得、設定します。</summary>
        public double Left
        {
            get { return _Left; }
            set { SetAndRaisePropertyChanged(ref _Left, value); }
        }
        #endregion

        #region Top変更通知プロパティ
        private double _Top;
        /// <summary>ウィンドウ上端の座標を取得、設定します。</summary>
        public double Top
        {
            get { return _Top; }
            set { SetAndRaisePropertyChanged(ref _Top, value); }
        }
        #endregion

        #region Topmost変更通知プロパティ
        private bool _Topmost;
        /// <summary>ウィンドウを最前面に表示するかを取得、設定します。</summary>
        public bool Topmost
        {
            get { return _Topmost; }
            set
            {
                if (_Topmost == value)
                    return;
                _Topmost = value;
                //サクッと反映: どうせ軽い設定ファイルなので大丈夫
                var csr = CommonSettingRecord.Load();
                csr.Topmost = _Topmost;
                csr.Save();
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>ウィンドウ右端の座標を取得します。</summary>
        public double Right => Left + Width;
        /// <summary>ウィンドウ下端の座標を取得します。</summary>
        public double Bottom => Top + Height;

        #region CanClose変更通知プロパティ
        private bool _canClose = false;
        /// <summary>ウィンドウを閉じて大丈夫かを取得、設定します。</summary>
        public bool CanClose
        {
            get { return _canClose; }
            set { SetAndRaisePropertyChanged(ref _canClose, value); }
        }
        #endregion

        #region Content変更通知プロパティ
        private object _Content;
        /// <summary>ウィンドウへの表示内容を取得、設定します。</summary>
        public object Content
        {
            get { return _Content; }
            set
            {
                if (_Content == value)
                    return;
                _Content = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>「終了」ボタンが押されたことを通知する</summary>
        public event EventHandler CloseRequested;

        /// <summary> メインウィンドウが閉じたのをモデル側に通知する </summary>
        public event EventHandler Closing;

        #endregion

        #endregion

        #region プロパティ

        #region IsDragged変更通知プロパティ
        private bool _IsDragged;
        /// <summary>ウィンドウをドラッグしているかを取得、設定します</summary>
        public bool IsDragged
        {
            get { return _IsDragged; }
            set { SetAndRaisePropertyChanged(ref _IsDragged, value); }
        }
        #endregion

        #region AvailableCharacters変更通知プロパティ
        private MenuItem[] _AvailableCharacters;
        /// <summary>利用可能なキャラクターの一覧を取得、設定します。</summary>
        public MenuItem[] AvailableCharacters
        {
            get { return _AvailableCharacters; }
            set
            {
                if (_AvailableCharacters == value)
                    return;
                _AvailableCharacters = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        //CAUTION: WindowプロパティはあくまでスクリプトAPIの便宜を図るもの。
        //アセンブリ内でコレを使ってMVVMを破壊してはいけない。
        #region Window変更通知プロパティ
        private Window _Window;
        /// <summary>ビューを取得、設定します。</summary>
        public Window Window
        {
            get { return _Window; }
            set
            {
                if (_Window == value)
                    return;
                _Window = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #endregion

        #region コマンド

        #region CloseCommand
        private ViewModelCommand _CloseCommand;
        /// <summary>ウィンドウを閉じるコマンドを取得します。</summary>
        public ViewModelCommand CloseCommand
            => _CloseCommand ?? (_CloseCommand = new ViewModelCommand(() => CloseRequested?.Invoke(this, EventArgs.Empty)));

        /// <summary>ビューを閉じます。</summary>
        public void Close()
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                CanClose = true;
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            });
        }

        #endregion

        #region ShowSettingCommand
        private ViewModelCommand _ShowSettingCommand;
        /// <summary>設定画面を開くコマンドを取得します。</summary>
        public ViewModelCommand ShowSettingCommand
            => _ShowSettingCommand ?? (_ShowSettingCommand = new ViewModelCommand(ShowSetting));

        /// <summary>設定画面を表示します。</summary>
        private void ShowSetting()
        {
            var vm = new SettingWindowViewModel(_modelCore.CharacterSetting);
            var message = new TransitionMessage(vm, "Show/SettingWindow");
            Messenger.Raise(message);
        }
        #endregion

        #region ToggleTopmostCommand
        private ViewModelCommand _ToggleTopmostCommand;
        /// <summary>最前面表示を切り替えるコマンドを取得します。</summary>
        public ViewModelCommand ToggleTopmostCommand
            => _ToggleTopmostCommand ?? (_ToggleTopmostCommand = new ViewModelCommand(ToggleTopmost));

        private void ToggleTopmost() => Topmost = !Topmost;

        #endregion

        #region CloseCanceledCommand
        private ViewModelCommand _CloseCanceledCommand;
        /// <summary>ウィンドウを閉じるコマンドを取得します。</summary>
        public ViewModelCommand CloseCanceledCommand
            => _CloseCanceledCommand ?? (_CloseCanceledCommand = new ViewModelCommand(CloseCanceled));

        private void CloseCanceled() => CloseRequested?.Invoke(this, EventArgs.Empty);
        #endregion

        #endregion



        /// <summary> 自前で用意したリソース解放の為の起点 </summary>
        /// <param name="disposing">継承元が使うフラグ</param>
        protected override void Dispose(bool disposing)
        {
            Closing?.Invoke(this, EventArgs.Empty);
            base.Dispose(disposing);
        }

        private void OnCharacterSelected(string characterName)
        {
            CharacterSelected?.Invoke(this, new CharacterSelectedEventArgs(characterName));
        }

        private ModelCore _modelCore;

    }


}
