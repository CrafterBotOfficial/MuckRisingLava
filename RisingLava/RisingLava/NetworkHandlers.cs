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
        internal static void StartGame(byte[] data)
        {
            new GameObject("LavaController").AddComponent<LavaController>();
            "I got a packet from the master client, I am so special:) | Packet is:[Start game]".Log();
        }
        [OffroadPacket]
        internal static void OnGlobalMessageRecieved(BinaryReader reader)
        {
            string message = reader.ReadString();
            bool IsRed = reader.ReadBoolean();
            message.ShowChatMessage(IsRed);
        }
        [OffroadPacket]
        internal static void LavaSync(byte[] data)
        {
            float DesiredY = System.BitConverter.ToSingle(data, 0);
            LavaController.LavaObject.position = new UnityEngine.Vector3(0, DesiredY, 0);
        }
    }
}
