﻿using System;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public abstract class CustomTeam
    {
        public CustomRole Role;
        public Teams Team;
        public Color Color = new(0, 0, 0);
        public Color MapBackColor;
        public bool HasKillButton = false;
        public bool CanUseVent = true;
        public bool CanUseAdmin = true;
        public bool CanUseCamera = true;
        public bool CanUseVital = true;
        public bool CanUseDoorlog = true;
        public bool CanUseBinoculars = true;
        public bool CanRepairSabotage = true;
        public bool CanUseVentMoving = true;
        public bool HasTask = true;
        public bool ImpostorMap = false;
        public bool AdminMap = false;
        public bool ShowingMapAllowedToMove = true;
        public bool ShowingAdminIncludeDeadBodies = true;
        public bool LightDirectional = false;
        public string ColoredTeamName => ColoredText(Color, Translation.GetString("team." + Team.ToString() + ".name"));
        public string RoleName => Translation.GetString("team." + Team.ToString() + ".name");
        public string ShortRoleName => Translation.GetString("team." + Team.ToString() + ".sname");
        public string ColoredShortTeamName => ColoredText(Color, Translation.GetString("team." + Team.ToString() + ".sname"));
        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("team." + Team.ToString() + ".intro")]));
        public string Description => Translation.GetString("team." + Team.ToString() + ".description");
        public string WinConditionTeam => Translation.GetString("wincondition.pre", [Translation.GetString("team." + Team.ToString() + ".wincondition")]);
        public abstract bool WinCheck();
        public virtual bool AdditionalWinCheck(Teams winteam) { return false; }
        public virtual bool WasExiled() { return false; }
        public virtual Tuple<ChangeLightReason,float> GetLightMod(ShipStatus shipStatus, float num)
        {
            float ImpostorLightMod = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            /*|| (Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId && Jackal.hasImpostorVision))*/


            return Tuple.Create(ChangeLightReason.None, shipStatus.MaxLightRadius * ImpostorLightMod);
            //return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod;

        }
        public virtual Tuple<ChangeLightReason, float> GetOtherLight(PlayerControl pc,ShipStatus shipStatus, float num)
        {
            return Tuple.Create(ChangeLightReason.None, -1f);
        }
    }
}
