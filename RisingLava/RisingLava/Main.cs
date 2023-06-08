using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace RisingLava
{
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.risinglava",
            NAME = "Rising Lava",
            VERSION = "1.0.2";
        internal const string
            RisingLavaCallsign = "<color=#00cc33>RisingLava</color>",
            RisingLavaWarningCallsign = "<color=#eed202>Warning:</color>";

        internal static Main Instance;
        internal ManualLogSource manualLogSource => Logger;

        internal Transform LavaTransform;
        internal GameObject LavaObject;

        internal static ConfigEntry<float> LavaSpeed;

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo("Loaded " + NAME);

            var Config = new ConfigFile(Paths.ConfigPath + "/RisingLava.cfg", true);
            LavaSpeed = Config.Bind("Lava", "LavaSpeed", 5f, "How fast the lava rises. This value will be synced with the master/server.");

            new HarmonyLib.Harmony(GUID).PatchAll();
        }

        private async void Start()
        {
            // Assemble lava
            const int AmountOfParticleCreators = 5;
            LavaObject = GameObject.Instantiate(await LoadAsset("lava"));
            GameObject LavaParticlePrefab = await LoadAsset("partical");
            for (int i = 0; i < AmountOfParticleCreators; i++)
            {
                Transform particleSystem = GameObject.Instantiate(LavaParticlePrefab, LavaObject.transform).transform;
                particleSystem.localPosition = Vector3.zero;
            }

            LavaTransform = LavaObject.transform;
            DontDestroyOnLoad(LavaObject);
        }

        private AssetBundle _assetBundle;
        private async Task<GameObject> LoadAsset(string Name)
        {
            if (_assetBundle == null)
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RisingLava.Resources.assets"))
                {
                    byte[] bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bytes);

                    new WaitUntil(() => assetBundleCreateRequest.isDone);

                    _assetBundle = assetBundleCreateRequest.assetBundle;
                }
            AssetBundleRequest requestLoad = _assetBundle.LoadAssetAsync(Name);
            new WaitUntil(() => requestLoad.isDone);
            return (GameObject)requestLoad.asset;
        }
    }
}
