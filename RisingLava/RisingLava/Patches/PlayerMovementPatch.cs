using HarmonyLib;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(PlayerMovement))]
    internal class PlayerMovementPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static async void HookAwake()
        {
            try
            {
                /* This script will deterime if the 
                 * gamemode is LAVA and make a request 
                 * to the lava controller script to start the game */
            }
            catch
            {

            }
        }
        private enum GameType
        {
            NotCustom,
            Lava
        }
    }
}
