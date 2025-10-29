using System.Collections.Generic;
using SodaCraft.Localizations;
using UnityEngine;

namespace M14BigMan
{
    public static class M14Config
    {
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
            [SystemLanguage.Chinese] = "制式精准战斗步枪，虽然是全自动，但还有很强的威力。",
            [SystemLanguage.ChineseSimplified] = "制式精准战斗步枪，虽然是全自动，但还有很强的威力。",
            [SystemLanguage.ChineseTraditional] = "制式精準戰鬥步槍，雖然是全自動，但還有很強的威力。",
            [SystemLanguage.English] = "Standard precision combat rifle, although fully automatic, still has strong power.",
            [SystemLanguage.Japanese] = "制式精密戦闘ライフル、フルオートですが、依然として強力な威力があります。",
            [SystemLanguage.Korean] = "제식 정밀 전투 소총, 완전 자동이지만 여전히 강력한 위력을 가지고 있습니다.",
            [SystemLanguage.Russian] = "Стандартная точная боевая винтовка, хотя полностью автоматическая, все еще обладает сильной мощностью.",
        };
        
        // 默认描述
        private const string DefaultDescription = "Standard precision combat rifle, although fully automatic, still has strong power.";
        
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