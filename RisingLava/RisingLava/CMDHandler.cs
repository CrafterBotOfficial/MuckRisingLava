using RisingLava.Util;

namespace RisingLava
{
    internal class CMDHandler
    {
        internal static void startlava()
        {
            if (!IsMaster())
                return;

            // Send request to the lavacontroller to start the game
            if (!LavaController.StartGame())
                "ERROR:Failed to start game, please reset and try again. If the problem persists please contact the author(Crafterbot)".ShowChatMessage(true);
        }
        internal static void setspeed(float NewSpeed)
        {
            "Configuring speed...".ShowChatMessage();
            Main.LavaSpeed.Value = NewSpeed;
            "Configured speed!".ShowChatMessage();

            if (!IsMaster())
                "NOTE:Since you are not the host of this lobby changing the speed will not effect anything.".ShowChatMessage(true);
        }

        internal static bool IsMaster(bool ShowErrorIfNot = true, string ErrorText = "You must be the master-client<b>(host)</b> to execute this command.")
        {
            bool _IsMaster = LocalClient.serverOwner;
            if (!_IsMaster)
                ErrorText.ShowChatMessage(true);
            return _IsMaster;
        }
    }
}
