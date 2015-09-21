using System;
using System.Windows.Threading;

namespace Harriet.Models.Core
{
    /// <summary> 実装によらないタイマーを表したインターフェース </summary>
    public interface ITimer
    {
        /// <summary> タイマーによる更新を開始します。 </summary>
        void Start();

        /// <summary> タイマーによる更新を停止します。</summary>
        void Stop();

        /// <summary>タイマーが指定した時間だけ経過すると発生します。</summary>
        event EventHandler Tick;
        
    }

    /// <summary> System.Windows.DispatcherTimerによるタイマーの実装 </summary>
    public class MainTimer : ITimer
    {
        /// <summary> タイマーの呼び出し間隔を指定してタイマーを初期化する </summary>
        /// <param name="intervalMillisec"></param>
        public MainTimer(double intervalMillisec)
        {
            _mainTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds((intervalMillisec))
            };
        }

        /// <summary> タイマーによる更新を開始する </summary>
        public void Start()
        {
            _mainTimer.Start();
        }

        /// <summary> タイマーによる更新を停止する </summary>
        public void Stop()
        {
            _mainTimer.Stop();
        }

        /// <summary> タイマー周期に応じて発生するイベント </summary>
        public event EventHandler Tick
        {
            add
            {
                _mainTimer.Tick += value;
                TickCopy += value;
            }
            remove
            {
                _mainTimer.Tick -= value;
                TickCopy -= value;
            }
        }


        /// <summary> タイマー待ちを使用せずにTick時の挙動を呼び出す </summary>
        public virtual void UpdateWithoutTime()
        {
            if(TickCopy != null) TickCopy(this, EventArgs.Empty);
        }

        /// <summary> タイマーを使わない呼び出しを認可するための控え </summary>
        private event EventHandler TickCopy;

        /// <summary> タイマーの実体 </summary>
        private readonly DispatcherTimer _mainTimer;


    }

}
