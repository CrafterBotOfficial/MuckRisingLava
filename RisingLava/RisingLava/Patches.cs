using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RisingLava
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(ChatBox), "ChatCommand"), HarmonyPrefix, HarmonyWrapSafe]
        private static void Hook_ChatBox_ChatCommandSent(ChatBox __instance, string message)
        {
            if (ChatCommandManager.Instance == null)
                new ChatCommandManager();
            var instance = ChatCommandManager.Instance;

            string CMDName = message.Split(' ')[0].Replace("/", "");
            string[] Args = message.Split(' ').Skip(1).ToArray();

            instance.OnChatCommandSent(CMDName, Args);
        }
        [HarmonyPatch(typeof(LobbySettings), "Start"), HarmonyPostfix]
        private static void Hook_LobbySettings_Start(LobbySettings __instance)
        {
            Transform Parent = GameObject.Find("UI/Lobby/LobbySettings/SettingsPanel/Setting_Gamemdoe/").transform;
            for (int i = 0; i < Parent.childCount; i++)
                GameObject.Destroy(Parent.GetChild(i).gameObject);
            __instance.gamemodeSetting.AddSettings(0, Enum.GetNames(typeof(GameSettings.GameMode)).AddRangeToArray(new string[] { "Lava" }));
        }

        /* Networking */


        [HarmonyPatch(typeof(LocalClient), "InitializeClientData"), HarmonyPostfix]
        private static void Hook_LocalClient_InitClientData()
        {
            LocalClient.packetHandlers.Add((int)EventCodes.StartLava, new LocalClient.PacketHandler(Networking.NetworkManager.Client_StartGame));
            LocalClient.packetHandlers.Add((int)EventCodes.MoveLava, new LocalClient.PacketHandler(Networking.NetworkManager.Client_MoveLava));
            LocalClient.packetHandlers.Add((int)EventCodes.EndLava, new LocalClient.PacketHandler(Networking.NetworkManager.Client_EndGame));
            LocalClient.packetHandlers.Add((int)EventCodes.SendMessage, new LocalClient.PacketHandler(Networking.NetworkManager.Client_SendMessage));
        }
    }
}
