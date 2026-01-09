using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSR.Assets;
using TSR.Game.Role.Ability;
using TSR.Game.Role.Target;
using TSR.Patch;
using UnityEngine;

namespace TSR.Game.Role.Role.CustomRole
{
    public class Sheriff : RoleBase
    {

        public override string Role() => "tsr:sheriff";

        public override string Team() => "tsr:crewmate";

        public override Color32 RoleColor() => Helper.ColorFromColorcode("#f8cd46");

        public override void Init() { }
        public override void InitAbilityAndButton()
        {
            AddAbility( new Ability.Passive.ShowMapAbility(false, false, true, false));
            AddCustomButton(new Ability.Button.AdvancedButton.SheriffKillButton());
        }
        private const float KillCooldown = 30f;
        private const int RemainKillCount = -1; //-1は無制限
        private readonly AbilityRange KillRange = new(AbilityRange.Range.Medium);
        //private void SheriffKill() {
        //    var target = CustomButton.SetPlayerTarget(() => FPlayerControl.AllPlayer.Where(x => x.Team != TeamId.Crewmate).ToList(), CustomButton.GetRange(KillRange));
        //    if (target.Team != TeamId.Crewmate)
        //    {
        //        this.FPlayer.PC.RpcMurderPlayer(target.PC,true);
        //    }
        //    else
        //    {
        //        target.PC.RpcMurderPlayer(this.FPlayer.PC,true);
        //    }
        //}
        //private bool CanUseSheriffKill() => CustomButton.SetPlayerTarget(() => FPlayerControl.AllPlayer.Where(x => x.Team != TeamId.Crewmate).ToList(), CustomButton.GetRange(KillRange)) != null;

        public override Func<Color> TargetColor() => () => RoleColor();

        public override PlayerTargetBase? TargetBase()=>new ClassicSoloTarget(() => [.. FPlayerControl.AllPlayer]);

        public Sheriff()
        {
        }

        public override AbilityRange GetAbilityDistance() => new AbilityRange(AbilityRange.Range.Medium);
    }
}
