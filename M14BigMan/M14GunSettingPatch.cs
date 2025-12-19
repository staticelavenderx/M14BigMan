using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem.Items;
using UnityEngine;

namespace M14BigMan
{
    [HarmonyPatch(typeof(ItemSetting_Gun), "Start")]
    public class M14GunSettingPatch
    {
        static void Postfix(ItemSetting_Gun __instance)
        {
            var item = __instance.Item;
            if (item == null) return;

            // 调试输出：所有枪支的基本信息
            // UnityEngine.Debug.Log($"[M14 Debug] 枪支配置初始化: {item.DisplayName} (TypeID: {item.TypeID}, triggerMode: {__instance.triggerMode})");
            
            if (item.TypeID == 787)
            {
                HandleM14(__instance, item);
            }
            
            
        }
        
        //处理M14：修改开火模式和插槽配置
        private static void HandleM14(ItemSetting_Gun gunSetting, ItemStatsSystem.Item item)
        {
            UnityEngine.Debug.Log($"[M14 Debug] 找到M14! TypeID={item.TypeID}, 当前开火模式={gunSetting.triggerMode}");
            
            // 1. 修改开火模式
            // var oldMode = gunSetting.triggerMode;
            // gunSetting.triggerMode = ItemSetting_Gun.TriggerModes.auto;
            // UnityEngine.Debug.Log($"[M14 Mod] 已修改M14开火模式: {oldMode} → {gunSetting.triggerMode}");
            
            // 2. 修改插槽配置
            ModifyM14Slots(item);
            
            // 3. 修改价值（如果配置了自定义价值）
            int originalValue = item.Value;
            if (M14Config.CustomValue > 0)
            {
                item.Value = M14Config.CustomValue;
                UnityEngine.Debug.Log($"[M14 Mod] 已修改M14价值: {originalValue} → {item.Value}");
            }
            else
            {
                UnityEngine.Debug.Log($"[M14 Debug] M14使用原始价值: {item.Value}");
            }
            
            // 4. 调试输出：插槽信息
            DebugSlots(item, "M14");
        }
        
        // ==================== 插槽修改核心逻辑 ====================
        /// <summary>
        /// 修改M14的插槽配置：从5个扩展到7个
        /// 原始插槽：Scope、Muzzle、Grip、Mag、Special
        /// 修改后：Scope、Muzzle、Grip、Stock、Tec、Mag、Special
        /// </summary>
        internal static void ModifyM14Slots(ItemStatsSystem.Item item)
        {
            if (item.Slots == null || item.Slots.Count != M14Config.M14_ORIGINAL_SLOT_COUNT)
            {
                UnityEngine.Debug.LogWarning($"[M14 Mod] M14插槽配置异常（期望{M14Config.M14_ORIGINAL_SLOT_COUNT}个，实际{item.Slots?.Count ?? 0}个），无法修改");
                return;
            }
            
            // 创建Stock插槽
            var stockSlot = CreateSlotWithTags("Stock", M14Config.SlotTags.StockRequireTags, M14Config.SlotTags.StockExcludeTags);
            
            // 创建Tec插槽
            var tecSlot = CreateSlotWithTags("Tec", M14Config.SlotTags.TecRequireTags, M14Config.SlotTags.TecExcludeTags);
            
            if (stockSlot == null || tecSlot == null)
            {
                UnityEngine.Debug.LogError("[M14 Mod] 创建插槽失败，无法添加新插槽");
                return;
            }
            
            // 插入插槽到正确位置
            item.Slots.list.Insert(3, stockSlot);
            item.Slots.list.Insert(4, tecSlot);
            
            // 初始化新插槽
            stockSlot.Initialize(item.Slots);
            tecSlot.Initialize(item.Slots);
            
            UnityEngine.Debug.Log($"[M14 Mod] 已为M14添加插槽: Stock和Tec，总插槽数: {item.Slots.Count}/{M14Config.M14_TARGET_SLOT_COUNT}");
        }
        
        /// <summary>
        /// 创建带有Tag配置的插槽
        /// </summary>
        private static ItemStatsSystem.Items.Slot CreateSlotWithTags(string slotKey, string[] requireTagNames, string[] excludeTagNames)
        {
            try
            {
                // 创建基础插槽
                var slot = new ItemStatsSystem.Items.Slot(slotKey);
                
                // 设置requireTags
                if (requireTagNames != null && requireTagNames.Length > 0)
                {
                    foreach (var tagName in requireTagNames)
                    {
                        var tag = FindTagByName(tagName);
                        if (tag != null)
                        {
                            slot.requireTags.Add(tag);
                            UnityEngine.Debug.Log($"[M14BigMan Mod] 为插槽{slotKey}添加requireTag: {tagName}");
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning($"[M14BigMan Mod] 警告：找不到requireTag '{tagName}'，插槽{slotKey}可能无法正常工作");
                        }
                    }
                }
                
                // 设置excludeTags
                if (excludeTagNames != null && excludeTagNames.Length > 0)
                {
                    foreach (var tagName in excludeTagNames)
                    {
                        var tag = FindTagByName(tagName);
                        if (tag != null)
                        {
                            slot.excludeTags.Add(tag);
                            UnityEngine.Debug.Log($"[M14BigMan Mod] 为插槽{slotKey}添加excludeTag: {tagName}");
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning($"[M14BigMan Mod] 警告：找不到excludeTag '{tagName}'，插槽{slotKey}可能无法正常工作");
                        }
                    }
                }
                
                return slot;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[M14BigMan Mod] 创建插槽{slotKey}时出错: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 通过名称查找Tag对象
        /// </summary>
        private static Tag FindTagByName(string tagName)
        {
            try
            {
                var allTags = Resources.FindObjectsOfTypeAll<Tag>();
                foreach (var loadedTag in allTags)
                {
                    if (loadedTag.name == tagName)
                    {
                        UnityEngine.Debug.Log($"[M14BigMan Mod] 通过FindObjectsOfTypeAll找到Tag: {tagName}");
                        return loadedTag;
                    }
                }
                
                UnityEngine.Debug.LogWarning($"[M14BigMan Mod] 无法找到Tag: {tagName}");
                return null;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[M14BigMan Mod] 查找Tag {tagName} 时出错: {ex.Message}");
                return null;
            }
        }
        
        
        private static void DebugSlots(ItemStatsSystem.Item item, string gunName)
        {
            if (item.Slots == null)
            {
                UnityEngine.Debug.Log($"[M14BigMan Debug] {gunName}没有插槽集合");
                return;
            }
            
            int slotCount = item.Slots.Count;
            UnityEngine.Debug.Log($"[M14BigMan Debug] {gunName}插槽信息: 共{slotCount}个插槽");
            
            for (int i = 0; i < slotCount; i++)
            {
                var slot = item.Slots.GetSlotByIndex(i);
                if (slot == null)
                {
                    UnityEngine.Debug.Log($"[M14BigMan Debug]   插槽{i}: null");
                    continue;
                }
                
                string requireTagsStr = slot.requireTags != null ? string.Join(", ", slot.requireTags) : "无";
                string excludeTagsStr = slot.excludeTags != null ? string.Join(", ", slot.excludeTags) : "无";
                
                UnityEngine.Debug.Log($"[M14BigMan Debug] 插槽{i}: key={slot.Key}, requireTags=[{requireTagsStr}], excludeTags=[{excludeTagsStr}]");
            }
        }
    }
}