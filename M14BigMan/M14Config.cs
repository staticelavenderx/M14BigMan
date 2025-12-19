using System.Collections.Generic;
using SodaCraft.Localizations;
using UnityEngine;

namespace M14BigMan
{
    public static class M14Config
    {
        /// <summary>
        /// M14原始插槽数量（游戏更新后的默认值）
        /// </summary>
        public const int M14_ORIGINAL_SLOT_COUNT = 5;
        
        /// <summary>
        /// M14目标插槽数量（添加Stock和Tec后）
        /// </summary>
        public const int M14_TARGET_SLOT_COUNT = 7;
        
        public static int CustomValue = 17781;
        
        // 硬编码插槽Tag配置
        public static class SlotTags
        {
            // Stock插槽的Tag名称
            public static readonly string[] StockRequireTags = new string[] { "Stock", "GunType_AR" };
            public static readonly string[] StockExcludeTags = new string[] { };
            
            // Tec插槽的Tag名称
            public static readonly string[] TecRequireTags = new string[] { "TecEquip" };
            public static readonly string[] TecExcludeTags = new string[] { };
        }
        
        // 多语言描述配置
        private static readonly Dictionary<SystemLanguage, string> Descriptions = new Dictionary<SystemLanguage, string>
        {
            [SystemLanguage.Chinese] = "虽然伤害高，但是射速也很快鸭~",
            [SystemLanguage.ChineseSimplified] = "虽然伤害高，但是射速也很快鸭~",
            [SystemLanguage.ChineseTraditional] = "雖然傷害高，但射速也很快鴨~",
            [SystemLanguage.English] = "High damage, but fires quacking fast!",
            [SystemLanguage.Japanese] = "高ダメージだけど、発射速度もカモン早いよ！",
            [SystemLanguage.Korean] = "데미지는 높지만, 연사력이 덜덜 빠르덕!",
            [SystemLanguage.Russian] = "Урон высокий, но и скорострельность утиная быстрая!",
        };
        // 默认描述
        private const string DefaultDescription = "虽然伤害高，但是射速也很快鸭~";
        
        // 根据当前游戏语言获取描述
        public static string GetDescription()
        {
            SystemLanguage currentLanguage = LocalizationManager.CurrentLanguage;
            
            if (Descriptions.ContainsKey(currentLanguage))
            {
                return Descriptions[currentLanguage];
            }
            return DefaultDescription;
        }
    }
}