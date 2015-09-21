using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Livet.Commands;

namespace Harriet.ViewModels
{
    /// <summary>外部サイトへのリンクを表します。</summary>
    public class ExternalLinkViewModel : HarrietViewModelBase
    {
        //NOTE: コイツはモデルロジックを持ってるが量が少ないし独立性も高いのでわざわざM-VMに分ける必要ない
        private const string CreatorWebSiteUrl = @"http://www.baku-dreameater.net";
        private const string GitHubUrl = @"https://github.com/malaybaku/Harriet";
        private const string VideoMylistUrl = @"http://www.nicovideo.jp/mylist/50850728";

        /// <summary>製作者のページへ飛ぶコマンドを取得します。</summary>
        public ViewModelCommand GotoCreatorSiteCommand { get; } = new ViewModelCommand(() => NavigateToUrl(CreatorWebSiteUrl));
        /// <summary>GitHubのページへ飛ぶコマンドを取得します。</summary>
        public ViewModelCommand GotoGithubSiteCommand { get; } = new ViewModelCommand(() => NavigateToUrl(GitHubUrl));
        /// <summary>投稿動画マイリストページへ飛ぶコマンドを取得します。</summary>
        public ViewModelCommand GotoVideoMylistCommand { get; } = new ViewModelCommand(() => NavigateToUrl(VideoMylistUrl));

        /// <summary>指定されたURLを既定のソフト(普通はウェブブラウザ)で開きます。</summary>
        static void NavigateToUrl(string url) => Task.Run(() => Process.Start(new Uri(url).AbsoluteUri));

    }
}
