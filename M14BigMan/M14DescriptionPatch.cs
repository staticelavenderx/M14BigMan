using HarmonyLib;
using ItemStatsSystem;
using SodaCraft.Localizations;

namespace M14BigMan
{
    [HarmonyPatch(typeof(Item), "Description", MethodType.Getter)]
    public class M14DescriptionPatch
    {
        static void Postfix(Item __instance, ref string __result)
        {
            if (__instance.TypeID != 787)
                return;
            
            string customDesc = M14Config.GetDescription();
            if (!string.IsNullOrEmpty(customDesc))
            {
                string originalDesc = __result;
                __result = customDesc;
                
                // UnityEngine.Debug.Log($"[M14BigMan Mod] 已修改M14描述（语言：{LocalizationManager.CurrentLanguageDisplayName}");
                // UnityEngine.Debug.Log($"[M14BigMan Debug] 原始描述: {originalDesc}");
                // UnityEngine.Debug.Log($"[M14BigMan Debug] 新描述: {__result}");
            }
        }
    }
}
