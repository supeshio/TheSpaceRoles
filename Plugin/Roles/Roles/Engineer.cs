using UnityEngine;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Engineer : CustomRole
    {
        public Engineer()
        {
            Team= Teams.Crewmate;
            Role = Roles.Engineer;
            Color = Helper.ColorFromColorcode("#0028f5");
            CanUseVent = true;
            CanUseVentMoving = true;
        }

        public static CustomOption RepairTimes;
        public override void OptionCreate()
        {
            RepairTimes = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.template.temp", new CustomIntRange(0,20),1);

            Options = [RepairTimes];
        }
        CustomButton RepairButton;
        public override void HudManagerStart(HudManager hudManager)
        {
            //DestroyableSingleton<VentButton>.Instance.SetUsesRemaining(5);
            RepairButton = new CustomButton(
            hudManager: hudManager,
            name: "RepairButton",
            buttonPos: ButtonPos.Custom,
            keycode: KeyCode.F,
            maxTimer: 0,
            canUse: () => DestroyableSingleton<SabotageTask>.Instance?.didContribute==false?1:-1/*-1以外なら成功判定*/,
            sprite: Sprites.GetSpriteFromResources("ui.button.template.png", 100f),
            Onclick: () =>
            {
                ShipStatus.Instance.RepairCriticalSabotages();
                switch
                (ShipStatus.Instance.GetSabotageTask(SystemTypes.Reactor).TaskType)
                {
                    case TaskTypes.RestoreOxy:
                        //DestroyableSingleton<ShipStatus>.Instance.RpcRepairSystem(SystemTypes.Reactor,);
                        break;
                    default:
                        break;
                }
            },
            OnMeetingEnds: () =>
            {
                RepairButton.Timer = RepairButton.maxTimer;
            },
            buttonText: "Repair",
            false,
            remainUses: RepairTimes.GetIntValue());
        }
    }
}
