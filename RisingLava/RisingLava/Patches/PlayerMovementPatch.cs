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
                else if ((int)GameManager.gameSettings.gameMode == 4)
                {
                    Type = GameType.Hard;
                    "The lava is rising on HARD MODE".ShowChatMessage();
                }

                if (LocalClient.serverOwner && Type != GameType.NotCustom)
                {
                    int MinutesUntilStart = Random.Range(1, 5);

                    if (Type == GameType.Hard)
                        new GameObject().AddComponent<LavaController>();
                    else
                    {
                        OffroadPacketWriter offroadPacketWriter = Main.offroadPackets.WriteToAll(nameof(NetworkHandlers.RecieveMessage), Steamworks.P2PSend.Reliable);
                        offroadPacketWriter.Write($"The lava will start rising in [{MinutesUntilStart}] minutes!");
                        offroadPacketWriter.Send();

                        await Task.Delay(MinutesUntilStart * 60 * 1000);
                        new GameObject().AddComponent<LavaController>();
                    }
                    "Set custom speed for gamemode".Log();
                }
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
            Hard
        }
    }
}
