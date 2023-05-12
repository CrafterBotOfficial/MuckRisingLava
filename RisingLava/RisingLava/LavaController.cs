using RisingLava.Util;
using UnityEngine;

namespace RisingLava
{
    internal class LavaController : MonoBehaviour
    {
        private const int ParticalsSpawnersToSpawn = 5;

        internal static LavaController Instance;
        internal static Transform LavaObject;
        private void Awake()
        {
            Instance = this;
            "Loading game...".Log(BepInEx.Logging.LogLevel.Message);

            LavaObject = GameObject.Instantiate((GameObject)AssetLoader.GetAsset("lava")).transform;
            LavaObject.position = Vector3.up * 7;
            for (int i = 0; i < ParticalsSpawnersToSpawn; i++)
                GameObject.Instantiate((GameObject)AssetLoader.GetAsset("partical")).transform.SetParent(LavaObject.transform);

            "Loaded game".Log();
        }
        private void OnDestroy()
        {
            GameObject.Destroy(LavaObject);
        }

        /* Static fields */

        /// <returns>If the game was started as expected.</returns>
        internal static bool StartGame()
        {
            if (Instance != null)
                return false;
            new GameObject("LavaController").AddComponent<LavaController>();
            new GameObject("Master_LavaController").AddComponent<Master_LavaController>();

            Main.offroadPackets.SendToAllExcept(nameof(NetworkHandlers.StartGame), LocalClient.instance.myId, new byte[0], Steamworks.P2PSend.Reliable);
            Main.SendGlobalMessage("The lava has started rising!");
            return true;
        }
        internal static void EndGame()
        {
            if (LocalClient.serverOwner)
                GameObject.Destroy(GameObject.FindObjectOfType<Master_LavaController>());
        }
    }

    internal class Master_LavaController : MonoBehaviour
    {
        private int GoToHieght = 99999;
        /* Sync lava */
        private void FixedUpdate()
        {
            LavaController.LavaObject.position = Vector3.Lerp(LavaController.LavaObject.position, Vector3.up * GoToHieght, Main.LavaSpeed.Value / (1500 * GoToHieght));

            // make a byte array 
            float NewSyncedPosition = LavaController.LavaObject.position.y;
            byte[] bytes = new byte[4];
            System.Buffer.BlockCopy(System.BitConverter.GetBytes(NewSyncedPosition), 0, bytes, 0, 4);
            Main.offroadPackets.SendToAllExcept(nameof(NetworkHandlers.LavaSync), LocalClient.instance.myId, bytes, Steamworks.P2PSend.UnreliableNoDelay);
        }
    }
}
