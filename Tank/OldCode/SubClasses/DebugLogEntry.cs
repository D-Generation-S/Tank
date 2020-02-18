using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tank.Enums;

namespace Tank.Code.SubClasses
{
    public class DebugLogEntry
    {
        public DebugLog LogType
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public Color FontColor
        {
            get
            {
                Color cReturn = Color.Gray;

                switch (LogType)
                {
                    case DebugLog.Normal:
                        cReturn =  Color.White;
                        break;
                    case DebugLog.Error:
                        cReturn = Color.Red;
                        break;
                    case DebugLog.Info:
                        cReturn = Color.LightBlue;
                        break;
                    default:
                        cReturn = Color.Gray;
                        break;
                }
                return cReturn;
            }
        }
        public DebugLogEntry(string msg, DebugLog type)
        {
            LogType = type;
            Message = msg;
        }
    }
}
