using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace M14BigMan
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private const string Id = "Custom.M14";

        private Harmony? _harmony;

        private void OnEnable()
        {
            UnityEngine.Debug.Log("M14BigMan Mod已加载");
            _harmony = new Harmony(Id);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnDisable()
        {
            _harmony?.UnpatchAll(Id);
        }
    }
}
