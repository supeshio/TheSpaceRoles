using System;
using System.Runtime.CompilerServices;

namespace TSR
{
    public class Logger
    {
        public static void Info(string text, string tag = "", [CallerMemberName] string callerMethod = "") => TSR._Logger.LogInfo($"[{tag}:Info][{DateTime.Now}][{callerMethod}]{text}");

        public static void Warning(string text, string tag = "", [CallerMemberName] string callerMethod = "") => TSR._Logger.LogWarning($"[{tag}:Warning][{DateTime.Now}][{callerMethod}]{text}");

        public static void Error(string text, string tag = "", [CallerMemberName] string callerMethod = "") => TSR._Logger.LogError($"[{tag}:Error][{DateTime.Now}][{callerMethod}]{text}");

        public static void Fatal(string text, string tag = "", [CallerMemberName] string callerMethod = "") => TSR._Logger.LogFatal($"[{tag}:Fatel][{DateTime.Now}][{callerMethod}]{text}");

        public static void Message(string text, string tag = "", [CallerMemberName] string callerMethod = "") => TSR._Logger.LogMessage($"[{tag}][{DateTime.Now}][{callerMethod}]{text}");
    }
}