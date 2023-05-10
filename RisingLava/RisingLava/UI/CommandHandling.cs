using RisingLava.Util;
using Terrain.Packets;
using UnityEngine;

namespace RisingLava.UI
{
    internal static class CommandHandling
    {
        #region Variables

        #endregion

        private static void StartLava()
        {
            if (!LocalClient.serverOwner)
            {
                "You must be the Master Client to execute this command!".ShowChatMessage(true);
                return;
            }
            "Sending packet to all clients...".ShowChatMessage();
            GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<LavaController>();
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

        private static void SetSpeed(string arg0)
        {
            float Speed = float.Parse(arg0);
            Main.LavaSpeed.Value = Speed;
            $"Set lava speed:[{Speed}]".ShowChatMessage();
        }
    }
}
