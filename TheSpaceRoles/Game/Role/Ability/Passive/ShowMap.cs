namespace TSR.Game.Role.Ability.Passive;

public class ShowMapAbility : PassiveSkillBase
{
    private bool UseSabotage;
    private bool AlwaysUseAdmin;

    //bool UseAdmin;
    private bool CanMoveWhileShowingNormalMap;

    private bool CanMoveWhileShowingAdminMap;

    public ShowMapAbility(bool UseSabotage, bool AlwaysUseAdmin/*,bool UseAdmin*/, bool CanMoveWhileShowingNormalMap = true, bool CanMoveWhileShowingAdminMap = false)
    {
        this.UseSabotage = UseSabotage;
        this.AlwaysUseAdmin = AlwaysUseAdmin;
        //this.UseAdmin = UseAdmin;
        this.CanMoveWhileShowingAdminMap = CanMoveWhileShowingAdminMap;
        this.CanMoveWhileShowingNormalMap = CanMoveWhileShowingNormalMap;
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
    }

    public void MapOpened(ref MapBehaviour map, ref MapOptions opts)
    {
        switch (opts.Mode)
        {
            case MapOptions.Modes.Sabotage:
            case MapOptions.Modes.Normal:
                //クルーorサボマップ
                opts.Mode = UseSabotage ? MapOptions.Modes.Sabotage : MapOptions.Modes.Normal;
                opts.AllowMovementWhileMapOpen = CanMoveWhileShowingNormalMap;
                break;

            case MapOptions.Modes.CountOverlay:
                opts.AllowMovementWhileMapOpen = CanMoveWhileShowingAdminMap;
                //アドミン
                break;
        }
    }
}