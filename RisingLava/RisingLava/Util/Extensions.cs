using BepInEx.Logging;

namespace RisingLava.Util
{
    internal static class Extensions
    {
        internal static void Log(this object obj, LogLevel logLevel = LogLevel.Info)
        {
#if DEBUG
            Main.logSource.Log(logLevel, obj);
#endif
        }

        internal static void ShowChatMessage(this string message, bool Red = false, string color = "#00cc33")
        {
            "Showing chat message".Log();
            string prefix = Red ? "<color=#ff0000>" : "";
            string suffix = Red ? "</color>" : "";
            ChatBox.Instance.AppendMessage(LocalClient.instance.myId, prefix + message + suffix, $"<color={color}>{Main.NAME}</color>");
        }
    }
}
