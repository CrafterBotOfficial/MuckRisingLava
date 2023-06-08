using System;

namespace RisingLava
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class ChatCommandHandler : Attribute
    {
        internal string[] CMDName;
        internal ChatCommandHandler(params string[] CMDName)
        {
            this.CMDName = CMDName;
        }
    }
}
