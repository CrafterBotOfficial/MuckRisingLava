using RisingLava.Util;
using Terrain.Packets;
using UnityEngine;

namespace RisingLava
{
    internal class LavaController : MonoBehaviour
    {
        internal static LavaController Instance;
        public Transform LavaObj;
        public bool IsEnabled;
        private void Awake()
        {
            Instance = this;
            IsEnabled = true;
            if (LocalClient.serverOwner)
            {
                OffroadPacketWriter offroadPacketWriter = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.StartLava), Steamworks.P2PSend.Reliable);
                offroadPacketWriter.Write(Main.LavaSpeed.Value);
                offroadPacketWriter.Send();
            }
            "Loading bundle...".Log();
            LavaObj = GameObject.Instantiate(AssetLoader.GetAsset("lava") as GameObject).transform;
            LavaObj.transform.localPosition = Vector3.up * 7;

            for (int i = 0; i < 6; i++)
            {
                Transform ParticalSystem = GameObject.Instantiate(AssetLoader.GetAsset("partical") as GameObject).transform;
                ParticalSystem.position = Vector3.zero;
                ParticalSystem.SetParent(LavaObj);
            }
        }

        private void Update()
        {
            if (LocalClient.serverOwner && IsEnabled)
            {
                LavaController.Instance.LavaObj.position += Vector3.up * ((Main.LavaSpeed.Value / 1000));

                OffroadPacketWriter PacketWriter = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.UpdateLavaLocation), Steamworks.P2PSend.UnreliableNoDelay);
                PacketWriter.Write(LavaController.Instance.LavaObj.position.y);
                PacketWriter.Send();
            }
            if (PlayerMovement.Instance.transform.position.y - 1f < LavaObj.position.y && !PlayerStatus.Instance.IsPlayerDead())
                PlayerStatus.Instance.DealDamage(1, 1, true);
        }

        public void LavaLocomotion(float DesiredY)
        {
            LavaObj.position = new Vector3(0, DesiredY, 0);
        }
    }
}
