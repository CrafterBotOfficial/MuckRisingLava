/*
 Lava material by Rob luo
 From the Unity Asset Store
*/
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using RisingLava.Util;
using System.Reflection;
using Terrain.Packets;
using UnityEngine;

namespace RisingLava
{
    [BepInPlugin(GUID, NAME, VERSION), BepInDependency("Terrain.OffroadPackets")]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.risinglava",
            NAME = "Rising Lava",
            VERSION = "1.0.0";
        internal static ManualLogSource logSource;
        internal static OffroadPackets offroadPackets;

        internal static ConfigEntry<float> LavaSpeed { get; set; }

        private void Awake()
        {
            logSource = Logger;
            $"Init : {NAME}".Log();

            var Config = new ConfigFile(Paths.ConfigPath + "/RisingLava.cfg", true);
            LavaSpeed = Config.Bind("Lava", "LavaSpeed", 5f, "How fast the lava rises. This value will be synced with the master/server.");

            offroadPackets = OffroadPackets.Create<NetworkHandlers>();
            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
