using HarmonyLib;
using RisingLava.Util;
using System.Threading.Tasks;
using Terrain.Packets;
using UnityEngine;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(PlayerMovement))]
    internal class PlayerMovementPatch
    {
        [HarmonyPatch("Awake", MethodType.Normal)]
        [HarmonyPostfix]
        private static async void HookAwake()
        {
            try
            {
                GameType Type = GameType.NotCustom;
                if ((int)GameManager.gameSettings.gameMode == 3)
                    Type = GameType.Easy;

                if (LocalClient.serverOwner && Type != GameType.NotCustom)
                {
                    int MinutesUntilStart = Random.Range(1, 5);

                    OffroadPacketWriter offroadPacketWriter = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.RecieveMessage), Steamworks.P2PSend.Reliable);
                    offroadPacketWriter.Write($"The lava will start rising in [{MinutesUntilStart}] minutes!");
                    offroadPacketWriter.Send();

                    for (int minute = MinutesUntilStart; minute > 0; minute--)
                    {
                        await Task.Delay(60 * 1000);
                        OffroadPacketWriter offroadPacketWriter1 = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.RecieveMessage), Steamworks.P2PSend.Reliable);
                        offroadPacketWriter1.Write($"The lava will start rising in [{minute}] minutes!");
                        offroadPacketWriter1.Send();
                    }
                    new GameObject().AddComponent<LavaController>();
                }
                "Set custom speed for gamemode".Log();
            }
            catch
            {
                /* Do nothing */
            }
        }
        private enum GameType
        {
            NotCustom,
            Easy,
            //Hard
        }
    }
}
