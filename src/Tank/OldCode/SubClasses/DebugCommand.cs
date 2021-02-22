using System;

namespace Tank.Code.SubClasses
{
    public class DebugCommand
    {
        public string Command
        {
            get; set;
        }
        public Func<string, DebugLogEntry> Method
        {
            get; set;
        }

        public DebugCommand(string cmd, Func<string, DebugLogEntry> mthd)
        {
            Command = cmd;
            Method = mthd;
        }
    }
}
