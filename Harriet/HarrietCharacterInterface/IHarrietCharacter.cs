using System;
using System.Threading.Tasks;

namespace Harriet.CharacterInterface
{
    /// <summary>キャラとしての描画に必要な仕様を定義します。</summary>
    public interface IHarrietCharacter : IDisposable
    {
        /// <summary> 約60fpsで呼び出される関数です。描画内容の更新などに使えます。 </summary>
        void Update();

        /// <summary> スケーリングしない状態での幅 </summary>
        double DefaultWidth { get; }
        /// <summary> スケーリングしない状態での高さ </summary>
        double DefaultHeight { get; }

        /// <summary>キャラの幅を取得、設定します。</summary>
        double Width { get; set; }
        /// <summary>キャラの高さを取得、設定します。</summary>
        double Height { get; set; }

        /// <summary>描画内容のリフレッシュが必要かどうかを取得します。</summary>
        bool IsDrawNeeded { get; }

        /// <summary>
        /// キャラの向きを設定します。
        /// NOTE: 現状では本体側からDirectionの設定は特に行ってません。
        /// </summary>
        Direction Direction { set; }

        /// <summary>
        /// 描画を行い、描画結果を返します。
        /// 戻り値はWPFのContentControlクラスでContentプロパティへ配置されるので、
        /// ImageなどUIElementの派生物を返すのを推奨します。
        /// </summary>
        Task<object> Draw();

        /// <summary>
        /// 口パク値(0～5)を設定します。0が完全に閉じた状態で、5が完全に開いた状態を意味します。
        /// NOTE: 0～5という範囲設定は特に意味があるわけじゃないので将来的に変更する可能性があります。
        /// </summary>
        int LipSynchValue { set; }

    }

    /// <summary>キャラの向きを表します。現在使われていません</summary>
    public enum Direction
    {
        Left,
        Right
    }
}
