using System;
using System.Reactive.Linq;

using Harriet.Models.Scripting;

namespace Harriet.Models.Core
{
    /// <summary> Harrietのコアロジックを定めるクラスです。</summary>
    public class ModelCore
    {
        /// <summary>このプログラムにおけるメインタイマーの更新間隔を秒単位で表します。</summary>
        public const double TimerUpdateIntervalSec = 0.016;

        /// <summary>モデルを読み込み、メイン動作を開始します。</summary>
        public static ModelCore LoadAndStart(IMainWindow mainWindow, ICharacterSelector characterSelector, INadeable nadenade)
        {
            var modelCore = new ModelCore(mainWindow, characterSelector, nadenade);
            modelCore.Initialize(mainWindow);
            return modelCore;
        }

        /// <summary>キャラの設定です。</summary>
        internal CharacterSetting CharacterSetting { get; private set; }


        /// <summary>このモデルを適用するウィンドウを指定し、モデルを初期化します。</summary>
        /// <param name="mainWindow">キャラ表示に使うウィンドウ</param>
        /// <param name="characterSelector">キャラ変更通知を行う何か</param>
        /// <param name="nadenade">撫で値の取得</param>
        private ModelCore(IMainWindow mainWindow, ICharacterSelector characterSelector, INadeable nadenade)
        {
            _nadenade = nadenade;

            _timer = new MainTimer(TimerUpdateIntervalSec * 1000);
            _timer.Tick += OnTimerTick;

            Observable.FromEventPattern<CharacterSelectedEventArgs>(characterSelector, nameof(characterSelector.CharacterSelected))
                .Take(1)
                .Subscribe(e => OnChangeCharacter(e.EventArgs.CharacterName));

            _closeRequestObserving = Observable.FromEventPattern<EventArgs>(mainWindow, nameof(mainWindow.CloseRequested))
                .Take(1)
                .Subscribe(_ => OnCloseRequested());

            Observable.FromEventPattern<EventArgs>(mainWindow, nameof(mainWindow.Closing))
                .Take(1)
                .Subscribe(_ => OnClosing());
        }

        /// <summary>
        /// 起動後にモデルの動作を開始します。
        /// NOTE: エントリポイント的に動作する
        /// </summary>
        private void Initialize(IMainWindow mainWindow)
        {
            string characterName = CommonSettingRecord.Load().CharacterName;
            CharacterSetting = CharacterSetting.Load(characterName);

            _characterOperator = new HarrietCharacterOperator(
                characterName,
                mainWindow,
                CharacterSetting.CharacterAppearance
                );

            _scriptingOperator = new ScriptingOperator(
                characterName,
                mainWindow,
                _characterOperator.Character,
                CharacterSetting
                );

            Observable.FromEventPattern<EventArgs>(_scriptingOperator, nameof(_scriptingOperator.Initialized))
                  .Take(1)
                  .Subscribe(_ => _timer.Start());

            Observable.FromEventPattern<EventArgs>(_scriptingOperator, nameof(_scriptingOperator.Closed))
                .Take(1)
                .Subscribe(_ => mainWindow.Close());

            _scriptingOperator.Start();

            //タイマーは初期化スクリプトが読み終わってから稼働開始するのでここでは放置
            //_timer.Start();
        }

        /// <summary>キャラ変更を試み、可能なら実際に変更する</summary>
        /// <param name="characterName">変更先のキャラ名</param>
        private void OnChangeCharacter(string characterName)
        {
            bool success = CharacterChanger.ReserveChangeCharacter(characterName);
            if(success)
            {
                //NOTE: こう書くことで普通に「終了」ボタン押された状況を再現する
                _closeRequestObserving.Dispose();
                OnCloseRequested();
            }
        }
        
        /// <summary>フレーム毎の更新処理</summary>
        private async void OnTimerTick(object sender, EventArgs e)
        {
            if (_isTimerBusy) return;

            _isTimerBusy = true;

            //1. キャラクタの更新計算
            await _characterOperator.Update(_scriptingOperator.LipSynchValue);

            //2. 撫で値の更新
            _nadenade.Nadenade -= CharacterSetting.Nadenade.DecreasePerFrame;
            if (_nadenade.Nadenade < 0) _nadenade.Nadenade = 0.0;
            if (_nadenade.Nadenade > CharacterSetting.Nadenade.Max) _nadenade.Nadenade = CharacterSetting.Nadenade.Max;

            //3. スクリプト/チャット枠の更新
            _scriptingOperator.Update();

            _isTimerBusy = false;
        }

        /// <summary>メインウィンドウからのアプリケーション終了要求を処理</summary>
        private void OnCloseRequested() => _scriptingOperator.Request(ScriptNames.Close, ScriptPriority.Close);

        /// <summary>メインウィンドウが閉じる前のリソース解放処理</summary>
        private void OnClosing()
        {
            _timer.Stop();

            CharacterSetting.Save();

            _scriptingOperator.Dispose();
            _characterOperator.Dispose();
        }

        #region フィールド

        /// <summary>撫でた量を保持する</summary>
        private readonly INadeable _nadenade;

        /// <summary>「終了」処理について購読状況を管理する</summary>
        private IDisposable _closeRequestObserving;

        /// <summary>キャラ表示の処理担当</summary>
        private HarrietCharacterOperator _characterOperator;

        /// <summary>スクリプト処理担当</summary>
        private ScriptingOperator _scriptingOperator;

        /// <summary>フレーム毎の処理回すのに使うタイマー</summary>
        private readonly ITimer _timer;

        /// <summary>タイマーの更新処理中であるかどうか</summary>
        private bool _isTimerBusy;

        #endregion

    }
}
