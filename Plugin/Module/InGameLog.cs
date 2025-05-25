using System.Collections.Generic;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public static class InGameLog
    {
        public static ActionButton LogButton;
        public static List<Log> logs = [];
        /// <summary>
        /// InGameLogType
        /// </summary>
        public class Log
        {
            string message = "";
            string type = "";
            Color color = ColorFromColorcode("#afeeee");
            public Log(string message, string type, Color color)
            {
                this.message = message;
                this.type = type;
                this.color = color;
            }

        }
        public static void CreateLogButton()
        {
            GameObject button = new GameObject("Log");
            button.transform.SetParent(HudManager.Instance.transform);
            var render = button.AddComponent<SpriteRenderer>();
            //render.sprite = Sprites.GetSpriteFromResources()
            LogButton = button.AddComponent<ActionButton>();
        }
        public static void CreateLogMaster(string message, string type, Color color)
        {
            _ = new Log(message, type, color);
        }
        public static void CreateLogSystem(string message)
        {
            CreateLogMaster(message, "System", ColorFromColorcode("#a9a9a9"));
        }
        public static void CreateLogAbility(string message)
        {
            CreateLogMaster(message, "Ability", ColorFromColorcode("#ff4500"));
        }
    }
}