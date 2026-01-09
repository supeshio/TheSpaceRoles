using System;
using System.Linq;
using TSR.Game.Role.Ability;
using TSR.Game.Role.Target;
using UnityEngine;

namespace TSR.Game.Role.Role.CustomRole
{
    public class Impostor : RoleBase
    {
        public override string Role() => "tsr:impostor";

        public override string Team() => "tsr:impostor";

        public override Color32 RoleColor() => Palette.ImpostorRed;
        public override void Init()
        {
            //throw new System.NotImplementedException();
        }
        public override void InitAbilityAndButton()
        {
            AddAbility(new Ability.Passive.ShowMapAbility(true, false, true, false));
        }

        public override Func<Color> TargetColor() => () => RoleColor();

        public override PlayerTargetBase? TargetBase() => new ClassicSoloTarget(()=>FPlayerControl.AllPlayer.Where(x=>x.Team!="tsr:impostor").ToList());

        public Impostor()
        {
        }
        public override AbilityRange GetAbilityDistance()=> new AbilityRange(AbilityRange.Range.Medium);
    }
}