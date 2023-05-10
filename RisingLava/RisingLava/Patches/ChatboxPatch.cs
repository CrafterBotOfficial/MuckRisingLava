using HarmonyLib;
using RisingLava.Util;
using System.Collections.Generic;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(ChatBox))]
    internal class ChatboxPatch
    {
        [HarmonyPatch("ChatCommand", MethodType.Normal)]
        [HarmonyPostfix]
        private static void HookChatCommandSent(ChatBox __instance, string message)
        {
            "Command recieved".Log();

            string command = message.Split(' ')[0].ToLower();
            string[] Args = message.Split(' ');

            string MethodName = CustomCommands.ContainsKey(command) ? CustomCommands[command] : "";
            if (MethodName != "")
            {
                "Command found".Log();
                object[] MethodArgs = new object[Args.Length - 1];
                for (int i = 1; i < Args.Length; i++)
                    MethodArgs[i - 1] = Args[i];
                typeof(UI.CommandHandling).GetMethod(MethodName, AccessTools.all).Invoke(null, MethodArgs);
            }
        }

        private static Dictionary<string, string> CustomCommands = new Dictionary<string, string>()
        {
            { "/startlava", "StartLava" },
            { "/resetlava", "ResetLava" },
            { "/togglelava", "PauseOrStartLava" },
            { "/setspeed", "SetSpeed" },
        };
    }
}
