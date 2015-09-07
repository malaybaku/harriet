using System;

namespace Harriet.Models.Chat
{
    /// <summary>チャット枠の位置直し要求を定義します。</summary>
    public interface IChatWindowRelocateRequestable
    {
        event EventHandler RelocateRequested;
    }

    public class ChatWindowRelocateRequestor : IChatWindowRelocateRequestable
    {
        public void Request() => RelocateRequested?.Invoke(this, EventArgs.Empty);

        public event EventHandler RelocateRequested;
    }

    public enum RelativePosition
    {
        None,
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom
    }

    public class RelativePositionEventArgs : EventArgs
    {
        public RelativePositionEventArgs(RelativePosition rp)
        {
            RelativePosition = rp;
        }

        public RelativePosition RelativePosition { get; }
    }

}
