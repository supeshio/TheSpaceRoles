using System;
using UnityEngine;

namespace TSR.Game.Role.Role.CustomTeam
{
    public class CrewmateTeam : TeamBase
    {
        public override string Team => "tsr:crewmate";
        public override string DefaultRole => "tsr:crewmate";
        public override Color32 TeamColor() => Palette.CrewmateBlue;
        public override void Init()
        {
        }

    }
}