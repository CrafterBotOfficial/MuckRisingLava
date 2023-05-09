using HarmonyLib;
using RisingLava.Util;
using System.Collections.Generic;
using Terrain.Packets;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(ChatBox))]
    internal class ChatboxPatch
    {
        [HarmonyPatch("ChatCommand", MethodType.Normal)]
        [HarmonyPostfix]
        private static void HookChatCommandSent(ChatBox __instance, string message)
        {
            "Command recieved".Log();

            string command = message.Split(' ')[0].ToLower();
            string[] Args = message.Split(' ');

            string MethodName = CustomCommands.ContainsKey(command) ? CustomCommands[command] : "";
            if (MethodName != "")
            {
                "Command found".Log();
                object[] MethodArgs = new object[Args.Length - 1];
                for (int i = 1; i < Args.Length; i++)
                    MethodArgs[i - 1] = Args[i];
                typeof(ChatboxPatch).GetMethod(MethodName, AccessTools.all).Invoke(null, MethodArgs);
            }
        }

        #region Command handling
        private static void StartLava()
        {
            if (!LocalClient.serverOwner)
            {
                "You must be the Master Client to execute this command!".ShowChatMessage(true);
                return;
            }
            "Sending packet to all clients...".ShowChatMessage();
            OffroadPacketWriter Packets = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.StartLava), Steamworks.P2PSend.Reliable);
            Packets.Write(LocalClient.instance.myId);
            Packets.Send();
        }
        private static void ResetLava()
        {
            if (!LocalClient.serverOwner)
            {
                "You must be the Master Client to execute this command!".ShowChatMessage(true);
                return;
            }
            "Telling all clients to reset lava!".ShowChatMessage();
            OffroadPacketWriter Packets = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.ResetLava), Steamworks.P2PSend.Reliable);
            Packets.Write(true); // (Removed)Delete LavaController
            Packets.Send();
        }
        private static void PauseOrStartLava()
        {
            if (LocalClient.serverOwner)
            {
                bool NewEnabled = !LavaController.Instance.IsEnabled;
                LavaController.Instance.IsEnabled = NewEnabled;
                OffroadPacketWriter packetWriter = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.RecieveMessage), Steamworks.P2PSend.Reliable);
                packetWriter.Write(NewEnabled ? "Lava is now continuing to move." : "Lava has been paused by the Master Client!");
                packetWriter.Send();
            }
            else
                "You must be the master client to execute this command".ShowChatMessage(true);
        }
        #endregion

        private static Dictionary<string, string> CustomCommands = new Dictionary<string, string>()
        {
            { "/startlava", "StartLava" },
            { "/resetlava", "ResetLava" },
            { "/togglelava", "PauseOrStartLava" }
        };
    }
}
