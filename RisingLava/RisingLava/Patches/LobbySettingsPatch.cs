using HarmonyLib;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(LobbySettings))]
    internal class LobbySettingsPatch
    {
        [HarmonyPatch("Start", MethodType.Normal)]
        [HarmonyPostfix]
        private static void HookStart(LobbySettings __instance)
        {
            /*
             This method will add the LAVA gamemode button
            */
            /*   
             *   I do not like this code anymore:/
             Transform Parent = GameObject.Find("UI/Lobby/LobbySettings/SettingsPanel/Setting_Gamemdoe/").transform;
                for (int i = 0; i < Parent.childCount; i++)
                    GameObject.Destroy(Parent.GetChild(i).gameObject);
                __instance.gamemodeSetting.AddSettings(0, Enum.GetNames(typeof(GameSettings.GameMode)).AddItem());
            */
        }
    }
}
