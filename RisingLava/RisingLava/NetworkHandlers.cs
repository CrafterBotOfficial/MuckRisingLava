using RisingLava.Util;
using System.IO;
using Terrain.Packets;
using UnityEngine;

namespace RisingLava
{
    [OffroadPacket(Main.GUID)]
    internal class NetworkHandlers
    {
        [OffroadPacket]
        public static void StartLava(BinaryReader reader)
        {
            "Got packet from Master Client".Log();
            "Starting game".Log(BepInEx.Logging.LogLevel.Message);
            "Starting game...".ShowChatMessage();
            $"Lava Speed:[{reader.ReadSingle()}]".ShowChatMessage();

            if (!LocalClient.serverOwner)
                new GameObject().AddComponent<LavaController>();
        }
        [OffroadPacket]
        public static void UpdateLavaLocation(BinaryReader reader)
        {
            if (LocalClient.serverOwner)
                return; // This is handled in the server

            float DesiredY = reader.ReadSingle();
            LavaController.Instance.LavaLocomotion(DesiredY);
        }
        [OffroadPacket]
        public static void ResetLava(BinaryReader reader)
        {
            "Got packet from Master Client".Log();
            "Resetting game".Log(BepInEx.Logging.LogLevel.Message);
            "Resetting game...".ShowChatMessage();

            GameObject.Destroy(LavaController.Instance.LavaObj.gameObject);
            GameObject.Destroy(LavaController.Instance.gameObject);

            PlayerStatus.Instance.Respawn();
        }
        [OffroadPacket]
        public static void LavaEnable(BinaryReader reader)
        {
            "Got packet from Master Client".Log();
            "Setting lava on".Log(BepInEx.Logging.LogLevel.Message);
            "Setting lava on...".ShowChatMessage();
            LavaController.Instance.IsEnabled = reader.ReadBoolean();
        }
        [OffroadPacket]
        public static void RecieveMessage(BinaryReader reader)
        {
            string message = reader.ReadString();
            message.ShowChatMessage();
        }
    }
}
