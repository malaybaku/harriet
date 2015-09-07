using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Livet;
using Livet.Commands;

using Harriet.Models.Chat;
using Harriet.Models.Core;
using System.ComponentModel;
using Livet.Messaging.Windows;

namespace Harriet.ViewModels
{
    /// <summary>チャット枠に対応したビューモデルを表します。</summary>
    public class ChatWindowViewModel : HarrietViewModelBase
    {

        /// <summary>
        /// モデルおよび設定を用いてインスタンスを初期化します。
        /// </summary>
        /// <param name="model">必要な描画物を実際に用意するモデル</param>
        /// <param name="mainWindow">位置決めに使うための親ウィンドウ</param>
        /// <param name="shownCharacterNameSetting">表示名の設定</param>
        /// <param name="textSetting">テキストのフォント設定</param>
        /// <param name="colorSetting">色の設定</param>
        /// <param name="layoutSetting">配置の設定</param>
        public ChatWindowViewModel(
            IChatWindowModel model,
            IMainWindow mainWindow,
            IShownCharacterNameSetting shownCharacterNameSetting,
            IChatWindowTextSetting textSetting,
            IChatWindowColorSetting colorSetting
            )
        {
            Position = new ChatWindowPositionViewModel(model.Position);

            CharacterName = new ShownCharacterNameViewModel(shownCharacterNameSetting);
            TextFont = new ChatWindowTextViewModel(textSetting);
            Color = new ChatWindowColorViewModel(colorSetting);

            Appearance = new ChatWindowAppearanceViewModel(this);

            AssignToModel(model);
            SyncTopmostToMainWindow(mainWindow);
        }

        /// <summary>ビューの初期化時に実行される関数です。</summary>
        public void Initialize()
        {
        }

        #region プロパティ

        #region Topmost変更通知プロパティ
        private bool _Topmost;
        /// <summary>ウィンドウが最前面に表示されるかを取得、設定します。</summary>
        public bool Topmost
        {
            get { return _Topmost; }
            set { SetAndRaisePropertyChanged(ref _Topmost, value); }
        }
        #endregion

        #region CanClose変更通知プロパティ
        private bool _CanClose;
        /// <summary>ウィンドウを閉じる操作が許可されているかを取得、設定します。</summary>
        public bool CanClose
        {
            get { return _CanClose; }
            set { SetAndRaisePropertyChanged(ref _CanClose, value); }
        }
        #endregion

        #region Content変更通知プロパティ
        private object _Content;
        /// <summary>チャット枠に表示する内容を取得、設定します。</summary>
        public object Content
        {
            get { return _Content; }
            set
            {
                if (_Content == value)
                    return;
                _Content = value;
                DispatcherHelper.UIDispatcher.Invoke(() => RaisePropertyChanged());
            }
        }
        #endregion

        #endregion

        #region コマンド

        #region GoNextCommand
        private ViewModelCommand _GoNextCommand;
        /// <summary>「次へ」ボタンが押されたときの反応を表すコマンドを取得します。</summary>
        public ViewModelCommand GoNextCommand
            => _GoNextCommand ?? (_GoNextCommand = new ViewModelCommand(GoNext));

        private void GoNext() => GoNextRequested?.Invoke(this, EventArgs.Empty);
        #endregion

        #endregion

        /// <summary>「次へ」ボタンが押されると発生します。</summary>
        public event EventHandler GoNextRequested;

        private void Close()
        {
            CanClose = true;
            Messenger.Raise(new WindowActionMessage(WindowAction.Close));
        }

        #region 子ビューモデル

        /// <summary>表示位置やサイズを取得します。</summary>
        public ChatWindowPositionViewModel Position { get; }

        /// <summary>表示名の設定を取得します。</summary>
        public ShownCharacterNameViewModel CharacterName { get; }

        /// <summary>フォント設定を取得します。</summary>
        public ChatWindowTextViewModel TextFont { get; }

        /// <summary>色設定を取得します。</summary>
        public ChatWindowColorViewModel Color { get; }

        /// <summary>表示/非表示と枠線の色についての設定を取得します。</summary>
        public ChatWindowAppearanceViewModel Appearance { get; }

        #endregion

        private void AssignToModel(IChatWindowModel chatWindow)
        {
            PropertyChangedEventManager.AddHandler(
                chatWindow,
                (_, __) => Content = chatWindow.Content,
                nameof(chatWindow.Content));

            WeakEventManager<IChatWindowModel, EventArgs>.AddHandler(
                chatWindow,
                nameof(chatWindow.Closing),
                (_, __) => Close()
                );

            WeakEventManager<IChatWindowModel, EventArgs>.AddHandler(
                chatWindow,
                nameof(chatWindow.HideRequested),
                (_, __) => Appearance.HideCommand.Execute()
                );

            GoNextRequested += (_, __) => chatWindow.Flush();
        }

        private void SyncTopmostToMainWindow(IMainWindow window)
        {
            Topmost = window.Topmost;
            PropertyChangedEventManager.AddHandler(
                window,
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(window.Topmost))
                    {
                        Topmost = (sender as IMainWindow).Topmost;
                    }
                },
                string.Empty
                );
        }

    }

}
