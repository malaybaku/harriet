using System;
using System.IO;
using System.Diagnostics;

using System.Windows;

namespace Harriet.Models.Core
{
    /// <summary>キャラ変更操作を定義します。</summary>
    public static class CharacterChanger
    {
        /// <summary>アプリケーションの終了時に別のキャラを起動する操作を予約します。</summary>
        /// <param name="newCharacterName">変更先のキャラクター名</param>
        /// <returns>変更予約に成功した場合はtrue、失敗した場合はfalse</returns>
        public static bool ReserveChangeCharacter(string newCharacterName)
        {
            bool isChangeValid = CheckChangeCharacterValidity(newCharacterName);
            if (!isChangeValid)
            {
                return false;
            }

            var record = CommonSettingRecord.Load();
            record.CharacterName = newCharacterName;
            record.Save();

            //再起動を予約
            Application.Current.Exit += (_, __) =>
            {
                string thisExePath = Path.Combine(
                    Environment.CurrentDirectory,
                    "Harriet.exe"
                    );
                Process.Start(thisExePath);
            };

            return true;
            //NOTE: あくまで予約操作なのでアプリケーションの終了は呼び出し元にやらせる
        }

        /// <summary>キャラ変更をして問題ないかを確認し、問題がある場合はメッセージボックスによる通知を行います。</summary>
        /// <param name="newCharacterName">変更したいキャラの名前</param>
        /// <returns>キャラ変更して大丈夫な場合はtrue、そうでない場合はfalse</returns>
        public static bool CheckChangeCharacterValidity(string newCharacterName)
        {
            string errorMessage = string.Empty;
            Action showErrorAction = () => MessageBox.Show(
                    "指定したキャラの読み込み中にエラーが発生しました。スクリプトかMEFライブラリのうち意図した方のエラーを確認してください:\n\n" + errorMessage,
                    "Harriet キャラロード失敗",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );

            bool ironPythonLoadValid = true;
            try
            {
                IronPythonCharacterLoader.CheckCharacterValidity(newCharacterName);
            }
            catch (Exception ex)
            {
                ironPythonLoadValid = false;
                errorMessage += $"--IronPython:{ex.Message}--\n\n";
            }

            bool mefLoadValid = true;
            try
            {
                MEFCharacterLoader.CheckValidity(newCharacterName);
            }
            catch (Exception ex)
            {
                mefLoadValid = false;
                errorMessage += $"--MEF:{ex.Message}--\n\n";
            }

            if (!(ironPythonLoadValid || mefLoadValid))
            {
                showErrorAction();
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
