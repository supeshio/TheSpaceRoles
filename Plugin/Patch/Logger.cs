using System;
using System.Runtime.CompilerServices;
namespace TheSpaceRoles
{
    public class Logger
    {
        public static void Info(string text, [CallerFilePath] string tag = "", [CallerMemberName] string callerMethod = "") => TSR.Logger.LogInfo($"[{tag}:Info][{DateTime.Now}][{callerMethod}]{text}");
        public static void Warning(string text, [CallerFilePath] string tag = "", [CallerMemberName] string callerMethod = "") => TSR.Logger.LogWarning($"[{tag}:Warning][{DateTime.Now}][{callerMethod}]{text}");
        public static void Error(string text, [CallerFilePath] string tag = "", [CallerMemberName] string callerMethod = "") => TSR.Logger.LogError($"[{tag}:Error][{DateTime.Now}][{callerMethod}]{text}");
        public static void Fatel(string text, [CallerFilePath] string tag = "", [CallerMemberName] string callerMethod = "") => TSR.Logger.LogFatal($"[{tag}:Fatel][{DateTime.Now}][{callerMethod}]{text}");
        public static void Message(string text, [CallerFilePath] string tag = "", [CallerMemberName] string callerMethod = "") => TSR.Logger.LogMessage($"[{tag}][{DateTime.Now}][{callerMethod}]{text}");


    }


}