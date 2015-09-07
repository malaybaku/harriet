using System;

namespace Harriet.Models.Voice
{
    /// <summary>リップシンク用のデータを表します。</summary>
    public class LipSyncher
    {
        /// <summary>リップシンクに必要なデータをもとにインスタンスを初期化します。</summary>
        /// <param name="start">開始時刻</param>
        /// <param name="values">指示値の一覧</param>
        /// <param name="intervalSec">指示値一つあたりに割り当てられた時間間隔(秒)</param>
        public LipSyncher(DateTime start, int[] values, double intervalSec)
        {
            _start = start;
            _values = values;
            _intervalSec = intervalSec;
        }

        private readonly DateTime _start;
        private readonly int[] _values;
        private readonly double _intervalSec;

        /// <summary>音声の再生時間っぽいものを取得します。</summary>
        public double TotalTimeSecond => _values.Length * _intervalSec; 

        /// <summary>リップシンクに有効な値が取り出せるかどうかを取得します。</summary>
        public bool IsValid => (DateTime.Now - _start).TotalSeconds < TotalTimeSecond;

        /// <summary>現在のリップシンク指示値を取得します。取得できない場合は0(口閉じ)を返します。</summary>
        public int CurrentValue
        {
            get
            {
                int index = (int) ((DateTime.Now - _start).TotalSeconds / _intervalSec);
                
                if(index > 0 && index < _values.Length)
                {
                    return _values[index];
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary> リップシンクに必要なデータが入ったイベント引数 </summary>
    public class LipSynchEventArgs : EventArgs
    {
        /// <summary>リップシンクデータをもとにインスタンスを初期化します。</summary>
        /// <param name="lipSyncher">リップシンクの実データ</param>
        public LipSynchEventArgs(LipSyncher lipSyncher)
        {
            LipSyncher = lipSyncher;
        }
        /// <summary>リップシンクに用いるデータ</summary>
        public LipSyncher LipSyncher { get; }
    }

}
