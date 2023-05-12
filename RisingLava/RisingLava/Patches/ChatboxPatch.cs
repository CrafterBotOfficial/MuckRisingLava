using HarmonyLib;
using RisingLava.Util;

namespace RisingLava.Patches
{
    [HarmonyPatch(typeof(ChatBox))]
    internal class ChatboxPatch
    {
        [HarmonyPatch("ChatCommand", MethodType.Normal)]
        [HarmonyPostfix]
        private static void HookChatCommandSent(string message)
        {
            ("Command recieved + " + message).Log();

            string commandRaw = message.Split(' ')[0];
            string command = commandRaw.ToLower().Replace("/", "");
            string[] RawArgs = message.Split(' ');

            commandRaw.Log();
            command.Log(BepInEx.Logging.LogLevel.Message);

            object[] args = new object[RawArgs.Length - 1];
            for (int i = 1; i < args.Length; i++)
            {
                $"Converted arg:[{RawArgs[i]}]".Log();
                args[i] = RawArgs[i].Trim();
            }

            typeof(CMDHandler).GetMethod(command, AccessTools.all).Invoke(null, args);
        }
    }
}
