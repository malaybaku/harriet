using System;
using System.Threading;
using System.Threading.Tasks;

using System.Reactive.Linq;

namespace Harriet.Models.Chat
{
    /// <summary>IReducedChatWindowのシンプルな実装を表します。</summary>
    public class ChatWindowModel : HarrietNotifiableModelBase, IChatWindowModel
    {
        public ChatWindowModel(ChatWindowPositionModel positionModel)
        {
            Position = positionModel;
        }

        #region Content変更通知プロパティ
        private object _Content;
        /// <summary>チャット枠に表示する内容を取得、設定します。</summary>
        public object Content
        {
            get { return _Content; }
            private set
            {
                if (_Content == value)
                    return;
                _Content = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        /// <summary>テキストを表示します。</summary>
        /// <param name="text">表示内容となるテキスト</param>
        /// <param name="charPerSec">1文字あたりに表示される文字数(0以下の場合一気に全文を表示する)</param>
        public void RenderText(string text, double charPerSec)
        {
            //いったん内容を消す
            Content = string.Empty;

            if (charPerSec <= 0.0)
            {
                Content = text;
                return;
            }

            var interval = TimeSpan.FromSeconds(1.0 / charPerSec);

            //途中でFlushRequestedイベントが来るとすぐに操作が終わるように設定
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            Action flush = () =>
            {
                cts.Cancel();
                Content = text;
            };

            using (var ob = Observable.FromEventPattern(this, nameof(FlushRequested))
                                    .Take(1)
                                    .Subscribe(e => flush()))
            { 
                try
                {
                    while(true)
                    {
                        string t = Content as string;
                        if (t == null) break;
                        if (t.Length >= text.Length) break;

                        t += text[t.Length];
                        Content = t;
                        Task.Delay(interval, token).Wait(token);
                    }
                }
                catch (TaskCanceledException)
                {
                    //Delayがキャンセルされた場合に発生。想定通りなので握りつぶしてOK
                }
                catch (OperationCanceledException)
                {
                    //こちらはWaitがキャンセルされたという意味で発生しうるが、こっちも握りつぶしてOK
                }
            }
        }

        /// <summary>任意の内容を表示します。</summary>
        /// <param name="content">表示させたい内容</param>
        public void RenderContent(object content)
        {
            //いったん内容を消去してから再表示する: リフレッシュみたいなもん
            Content = string.Empty;
            Content = content;
        }

        /// <summary>表示途中のテキストを一気に最後まで表示します。</summary>
        public void Flush() => FlushRequested?.Invoke(this, EventArgs.Empty);

        public void Dispose() => Closing?.Invoke(this, EventArgs.Empty);

        public void Hide() => HideRequested?.Invoke(this, EventArgs.Empty);

        public ChatWindowPositionModel Position { get; }

        public event EventHandler Closing;
        public event EventHandler HideRequested;


        private event EventHandler FlushRequested;

    }

}
