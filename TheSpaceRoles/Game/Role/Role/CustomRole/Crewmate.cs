using System;
using TSR.Game.Role.Ability;
using UnityEngine;

namespace TSR.Game.Role.Role.CustomRole
{
    public class Crewmate : RoleBase
    {
        public override string Role() => "tsr:crewmate";

        public override string Team() => "tsr:crewmate";

        public override Color32 RoleColor() => Palette.CrewmateBlue;
        public override void InitAbilityAndButton()
        {
            Abilities.Add(new Ability.Passive.ShowMapAbility(false, false, true, false));
        }
        public override Func<Color> TargetColor() => () => RoleColor();

        public override PlayerTargetBase? TargetBase() => null;

        public Crewmate()
        {
        }
        public override AbilityRange GetAbilityDistance() => new AbilityRange(AbilityRange.Range.None);

        public override void Init()
        {
            //throw new NotImplementedException();
        }
    }
}