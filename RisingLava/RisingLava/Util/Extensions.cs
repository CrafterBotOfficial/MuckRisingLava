using BepInEx.Logging;

namespace RisingLava.Util
{
    internal static class Extensions
    {
        internal static void Log(this object obj, LogLevel logLevel = LogLevel.Info)
        {
#if DEBUG
            switch (logLevel)
            {
                case LogLevel.Info: Main.logSource.LogInfo(obj); break;
                case LogLevel.Warning: Main.logSource.LogWarning(obj); break;
                case LogLevel.Error: Main.logSource.LogError(obj); break;
                case LogLevel.Message: Main.logSource.LogMessage(obj); break;
            }
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
