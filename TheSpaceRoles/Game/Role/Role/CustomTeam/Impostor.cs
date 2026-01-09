using UnityEngine;

namespace TSR.Game.Role.Role.CustomTeam
{
    public class ImpostorTeam : TeamBase
    {
        public override string Team => "tsr:impostor";
        public override string DefaultRole => "tsr:impostor ";

        public override Color32 TeamColor() => Palette.ImpostorRed;

        public override void Init()
        {
        }
    }
}