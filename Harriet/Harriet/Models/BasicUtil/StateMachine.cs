
namespace Harriet.Models.BasicUtil
{
    /// <summary>汎用的なステートマシンを定義します。</summary>
    class StateMachine
    {
        public StateMachine(IState initial)
        {
            _state = initial;
        }

        IState _state;

        public void Update()
        {
            IState previous = _state;
            _state = _state.Update();
            if(_state != previous)
            {
                previous.Quit();
                _state.Enter();
            }
        }

    }

    /// <summary>ステートマシンの1状態を定義します。</summary>
    interface IState
    {
        /// <summary>状態に入ったときに行われる処理です。</summary>
        void Enter();
        /// <summary>状態内で更新処理を行い、次にとるべく状態を指示します。</summary>
        /// <returns>次の取るべき状態</returns>
        IState Update();
        /// <summary>状態を抜けたときに行われる処理です。</summary>
        void Quit();
    }
}