using HarmonyLib;
using RisingLava.Util;
using TMPro;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(UiSettings))]
    internal class UiSettingsPatch
    {
        [HarmonyPatch("UpdateSetting", MethodType.Normal)]
        [HarmonyPrefix]
        private static void HookUpdateSetting(UiSettings __instance, int i)
        {
#if DEBUG
            Traverse traverse = Traverse.Create(__instance);
            TextMeshProUGUI[] texts = traverse.Field("texts").GetValue() as TextMeshProUGUI[];
            texts[i].text.Log();
            if (texts[i].text == "Lava" || texts[i].text == "Lava Hard")
                "Custom gamemode".Log();
#endif
        }
    }
}
