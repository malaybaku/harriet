using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harriet.Models.Voice
{
    /// <summary>AquesTalk関連の定数文字列をまとめたクラスです。</summary>
    public static class AquesVoiceConstNames
    {
        #region DLLパス(個人的に勝手に決めたパス)
        public const string PathF1 = @"dll\AquesTalk\f1\AquesTalk.dll";
        public const string PathF2 = @"dll\AquesTalk\f2\AquesTalk.dll";
        public const string PathM1 = @"dll\AquesTalk\m1\AquesTalk.dll";
        public const string PathM2 = @"dll\AquesTalk\m2\AquesTalk.dll";
        public const string PathR1 = @"dll\AquesTalk\r1\AquesTalk.dll";
        public const string PathImd1 = @"dll\AquesTalk\imd1\AquesTalk.dll";
        public const string PathDvd = @"dll\AquesTalk\dvd\AquesTalk.dll";
        public const string PathJgr = @"dll\AquesTalk\jgr\AquesTalk.dll";
        #endregion

        #region DLL内の関数名
        public const string AquesTalk_Synthe = "AquesTalk_Synthe";
        public const string AquesTalk_FreeWave = "AquesTalk_FreeWave";
        #endregion

        #region こっちは割と適当: 表示名の設定
        public const string NameF1 = "女性1(AquesTalk)";
        public const string NameF2 = "女性2(AquesTalk)";
        public const string NameM1 = "男性1(AquesTalk)";
        public const string NameM2 = "男性2(AquesTalk)";
        public const string NameR1 = "ロボット(AquesTalk)";
        public const string NameImd1 = "中性(AquesTalk)";
        public const string NameDvd = "機械(AquesTalk)";
        public const string NameJgr = "特殊(AquesTalk)";
        #endregion

    }

}
