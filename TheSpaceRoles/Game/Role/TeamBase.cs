using System.Collections.Generic;
using UnityEngine;

namespace TSR.Game.Role
{
    /// <summary>
    /// 各チームの基底クラス
    /// </summary>
    public abstract class TeamBase
    {
        // そのチームの種類を設定
        public virtual string Team => "tsr:none";
        public virtual string DefaultRole => "tsr:none";

        public List<FPlayerControl> Players = [];
        /// <summary>
        /// チーム名
        /// </summary>
        public string TeamName => Translation.Get($"role.{Team.ToString()}.name"); // デフォルト実装として Teams 列挙型の名前を返す

        /// <summary>
        /// チームの説明
        /// </summary>
        public string TeamDescription => Translation.Get($"role.{Team.ToString()}.description"); // デフォルトの説明

        public abstract void Init();
        public virtual void OnGameEnd() { }
        public virtual void OnGameStart() { }
        public virtual void OnMeetingEnd() { }
        public virtual void OnMeetingStart() { }
        public virtual void OnPlayerDied() { }
        public virtual void OnPlayerJoined() { }
        public virtual void OnPlayerLeft() { }
        public virtual void OnRoleAssigned() { }

        /// <summary>
        /// チームに紐づく色
        /// </summary>
        public abstract Color32 TeamColor(); // デフォルトの色


        public static TeamBase GetTeamBase(string teamId) => TeamBaseRegister.GetTeamBase(teamId);
    }
}