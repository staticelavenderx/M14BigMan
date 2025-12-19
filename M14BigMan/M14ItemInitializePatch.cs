using HarmonyLib;
using UnityEngine;

namespace M14BigMan
{

    // M14物品初始化补丁
    [HarmonyPatch(typeof(ItemStatsSystem.Item), "Initialize")]
    public class M14ItemInitializePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ItemStatsSystem.Item __instance)
        {
            try
            {
                var gunSetting = __instance.GetComponent<ItemSetting_Gun>();
                if (gunSetting == null) return;
                
                if (__instance.TypeID != 787) return;

                // 检查插槽数量
                if (__instance.Slots == null)
                {
                    return;
                }

                // 如果已经是7个插槽，说明已经修复过了，跳过
                if (__instance.Slots.Count == M14Config.M14_TARGET_SLOT_COUNT)
                {
                    UnityEngine.Debug.Log($"[M14BigMan Mod] M14已有{M14Config.M14_TARGET_SLOT_COUNT}个插槽，跳过修复");
                    return;
                }

                // 如果不是5个插槽，说明配置异常
                if (__instance.Slots.Count != M14Config.M14_ORIGINAL_SLOT_COUNT)
                {
                    UnityEngine.Debug.LogWarning($"[M14BigMan Mod] M14插槽数量异常: {__instance.Slots.Count}");
                    return;
                }

                // 开始修复插槽
                UnityEngine.Debug.Log("[M14BigMan Mod] 检测到从存档加载的M14，开始修复插槽...");

                // 执行插槽修复（使用硬编码Tag配置）
                M14GunSettingPatch.ModifyM14Slots(__instance);
                UnityEngine.Debug.Log("[M14BigMan Mod] 已修复从存档加载的M14插槽");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[M14BigMan Mod] Item.Initialize补丁执行失败: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}