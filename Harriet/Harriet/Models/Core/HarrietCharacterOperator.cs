using System;
using System.ComponentModel;
using System.Threading.Tasks;

using System.Windows;

using Harriet.CharacterInterface;

namespace Harriet.Models.Core
{
    /// <summary>キャラ関連の表示状態をアプリケーション全体にわたって管理するオペレータを表します。</summary>
    public class HarrietCharacterOperator : IDisposable
    {
        public HarrietCharacterOperator(
            string characterName,
            IMainWindow mainWindow, 
            ICharacterAppearanceSetting setting)
        {
            _mainWindow = mainWindow;
            _setting = setting;
            LoadCharacter(characterName);
        }

        /// <summary>
        /// 描画の更新処理を行います。ビジー状態などで有効な描画が行われない場合nullを返します。
        /// </summary>
        public async Task Update(int lipSynchValue)
        {
            Character.LipSynchValue = lipSynchValue;
            Character.Update();
            if (Character.IsDrawNeeded && !_isDrawingBusy)
            {
                _isDrawingBusy = true;
                _mainWindow.Content = await Character.Draw();
                _isDrawingBusy = false;
            }
        }

        /// <summary>キャラそのものを表します。</summary>
        public IHarrietCharacter Character { get; private set; }

        public void Dispose() => Character.Dispose();

        private readonly IMainWindow _mainWindow;
        private readonly ICharacterAppearanceSetting _setting;

        private bool _isDrawingBusy;

        /// <summary>キャラ名によってキャラをロードします。</summary>
        /// <param name="characterName">読み込み対象のキャラ名</param>
        private void LoadCharacter(string characterName)
        {
            string errorMessage = string.Empty;
            try
            {
                Character = IronPythonCharacterLoader.LoadCharacter(characterName);
            }
            catch (Exception ex1)
            {
                errorMessage += "IronPython: " + ex1.Message + "\n\n";
                try
                {
                    Character = MEFCharacterLoader.Load(characterName);
                }
                catch(Exception ex2)
                {
                    errorMessage += "MEF: " + ex2.Message + "\n\n";
                    MessageBox.Show(
                        "キャラクターのロードに失敗したためアプリケーションが起動できません。アプリケーションを終了します。\n" + errorMessage,
                        "Harriet キャラクターの初期化に失敗しました",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
                    Environment.Exit(1);
                }
            }

            SyncSizeSetting(_setting);
        }

        /// <summary>サイズ設定を同期します。</summary>
        /// <param name="appearanceSetting">設定元</param>
        private void SyncSizeSetting(ICharacterAppearanceSetting setting)
        {
            ApplySizeSetting();
            PropertyChangedEventManager.AddHandler(
                setting,
                (_, __) => ApplySizeSetting(),
                nameof(setting.SizeScale)
                );
        }

        /// <summary>サイズの変更を適用します。</summary>
        private void ApplySizeSetting()
        {
            Character.Width = _setting.SizeScale * Character.DefaultWidth;
            Character.Height = _setting.SizeScale * Character.DefaultHeight;

            _mainWindow.Width = Character.Width;
            _mainWindow.Height = Character.Height;
        }

    }
}
