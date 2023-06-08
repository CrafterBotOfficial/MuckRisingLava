using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RisingLava
{
    internal class ChatCommandManager
    {
        internal static ChatCommandManager Instance;

        private delegate void CMDHandlerMethod(string[] Args);
        private Dictionary<string, CMDHandlerMethod> Handlers;

        internal ChatCommandManager()
        {
            Instance = this;

            Handlers = new Dictionary<string, CMDHandlerMethod>();
            foreach (var method in typeof(ChatCommandManager).GetMethods())
            {
                ChatCommandHandler chatCommandHandler = method.GetCustomAttribute<ChatCommandHandler>();
                if (chatCommandHandler == null)
                    continue;
                foreach (string CMDName in chatCommandHandler.CMDName)
                    Handlers.Add(CMDName.ToLower(), (CMDHandlerMethod)Delegate.CreateDelegate(typeof(CMDHandlerMethod), this, method));
            }
            Main.Instance.manualLogSource.LogInfo("Found " + Handlers.Count + " chat commands");
        }

        internal bool OnChatCommandSent(string commandName, string[] Args)
        {
            string Key = commandName.ToLower();
            if (Handlers == null && Handlers[Key] == null)
                return false;
            Handlers[Key].Invoke(Args);
            return true;
        }

        [ChatCommandHandler("StartLava", "BeginLava", "FuckingStartTheLavaNowOrIWillHateYouForever", "CrafterbotIsDaCoolest")]
        public void StartLavaCMDHandler(string[] Args)
        {
            Networking.NetworkManager.SendMessage("The lava is starting to rise!!!", false);
            Networking.NetworkManager.StartLava();
        }
        [ChatCommandHandler("adjustspeed", "setspeed", "lavaspeed")]
        public void AdjustLavaSpeedCMDHandler(string[] Args)
        {
#if DEBUG
            Args
                .ToList()
                .ForEach(x => Main.Instance.manualLogSource.LogMessage(x));
#endif

            if (Args.Length != 1 || !float.TryParse(Args[0], out float NewSpeed))
            {
                ChatBox.Instance.AppendMessage(-1, "Invalid command arguments, please use '/adjustspeed 10.5' instead.", Main.RisingLavaCallsign);
                return;
            }
            float OldSpeed = Main.LavaSpeed.Value;
            Main.LavaSpeed.Value = NewSpeed;
            if (!LocalClient.serverOwner)
                ChatBox.Instance.AppendMessage(-1, "You are *not* the master of the server, the lava speed is changed on your local configuration file. However it will not effect *this* game at all.", Main.RisingLavaWarningCallsign);
            ChatBox.Instance.AppendMessage(-1, $"Successfully changed the lava speed from {OldSpeed} too {NewSpeed}", Main.RisingLavaCallsign);
        }
        [ChatCommandHandler("ForceEndGame", "endgame")]
        public void ForceEndGame(string[] Args)
        {
            if (LocalClient.serverOwner)
            {
                Networking.NetworkManager.EndGame();
                return;
            }
            ChatBox.Instance.AppendMessage(-1, "You are *not* the master of the server, you can't end the game.", Main.RisingLavaWarningCallsign);
        }
        [ChatCommandHandler("pauselava", "setlavastatus")]
        public void PauseLava(string[] Args)
        {
            if (!LocalClient.serverOwner)
                return;
            if (Args.Length != 1 || !bool.TryParse(Args[0], out bool NewStatus))
            {
                ChatBox.Instance.AppendMessage(-1, "Invalid command arguments, please use '/pauselava true' or '/pauselava false' instead.", Main.RisingLavaCallsign);
                return;
            }
            Main.Instance.LavaObject.GetComponent<Behaviours.NetworkedObject>().enabled = !NewStatus;
        }
    }
}
