using RisingLava.Behaviours;
using Steamworks;
using UnityEngine;

namespace RisingLava.Networking
{
    public class NetworkManager
    {
        /* ==================Senders================== */

        public static void SendMessage(string Message, bool IsWarning)
        {
            using (Packet packet = new Packet((int)EventCodes.SendMessage))
            {
                packet.Write(Message);
                packet.Write(IsWarning);
                SendTCPDataToAll(packet, P2PSend.Reliable);
            }
        }

        public static void StartLava()
        {
            using (Packet packet = new Packet((int)EventCodes.StartLava))
                SendTCPDataToAll(packet, P2PSend.Reliable);
        }
        public static void EndGame()
        {
            using (Packet packet = new Packet((int)EventCodes.EndLava))
                SendTCPDataToAll(packet, P2PSend.Reliable);
        }
        public static void MoveLava(float NewY)
        {
            using (Packet packet = new Packet((int)EventCodes.MoveLava))
            {
                packet.Write(NewY);
                SendUDPDataToAll(packet);
            }
        }


        // Token: 0x06000664 RID: 1636 RVA: 0x00022274 File Offset: 0x00020474
        private static void SendTCPDataToAll(Packet packet, P2PSend Reliability)
        {
            packet.WriteLength();
            if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
            {
                for (int i = 1; i < Server.MaxPlayers; i++)
                {
                    Server.clients[i].tcp.SendData(packet);
                }
                return;
            }
            foreach (Client client in Server.clients.Values)
            {
                if (((client != null) ? client.player : null) != null)
                {
                    SteamPacketManager.SendPacket(client.player.steamId.Value, packet, Reliability, SteamPacketManager.NetworkChannel.ToClient);
                }
            }
        }

        // Token: 0x06000669 RID: 1641 RVA: 0x0002261C File Offset: 0x0002081C
        private static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
            {
                for (int i = 1; i < Server.MaxPlayers; i++)
                {
                    Server.clients[i].udp.SendData(packet);
                }
                return;
            }
            foreach (Client client in Server.clients.Values)
            {
                if (((client != null) ? client.player : null) != null)
                {
                    SteamPacketManager.SendPacket(client.player.steamId.Value, packet, P2PSend.UnreliableNoDelay, SteamPacketManager.NetworkChannel.ToClient);
                }
            }
        }

        /* =================Handlers================= */

        /// <summary>
        /// Server => Client
        /// Start the game on the local client
        /// </summary>
        public static void Client_StartGame(Packet packet)
        {
            Main.Instance.LavaObject.SetActive(true);
            Main.Instance.LavaObject.AddComponent<NetworkedObject>();
        }

        public static void Client_EndGame(Packet packet)
        {
            if (Main.Instance.LavaObject.TryGetComponent(out NetworkedObject component))
                GameObject.Destroy(component);
            Main.Instance.LavaObject.SetActive(false);
        }

        /// <summary>
        /// Server => Client
        /// Moves the local lava to the designated height
        /// </summary>
        public static void Client_MoveLava(Packet packet)
        {
            if (!Main.Instance.LavaObject.activeSelf || LocalClient.serverOwner)
                return;
            float NewY = packet.ReadFloat();
            Main.Instance.LavaObject.transform.position = UnityEngine.Vector3.up * NewY;
        }

        public static void Client_SendMessage(Packet packet)
        {
            string Message = packet.ReadString();
            bool IsWarning = packet.ReadBool();
            ChatBox.Instance.AppendMessage(-1, Message, IsWarning ? Main.RisingLavaWarningCallsign : Main.RisingLavaCallsign);
        }
    }
}
