using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class EvilHacker : CustomRole
    {
        public CustomButton HackButton;
        //public CustomButton HackAdminButton;
        public EvilHacker()
        {

            Team = Teams.Impostor;
            Role = Roles.EvilHacker;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
            AdminMap = true;
            ImpostorMap = true;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            HackButton = new CustomButton(
                __instance, "EvilHackerHackButton"
                ,
                ButtonPos.Custom,
                KeyCode.F,
                30,
                () => CustomButton.SetTarget(),
                Sprites.GetSpriteFromResources("ui.button.evilhacker_hack.png", 100f),
                () =>
                {
                    var pc = GetPlayerControlFromId(CustomButton.SetTarget());
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(pc);
                    writer.EndRpc();
                    HackPlayer(PlayerId, pc.PlayerId);
                },
                () =>
                {
                    HackButton.Timer = HackButton.maxTimer;
                },
                "Hack",
                false);
            Logger.Info("button:EvilHackerButton");

        }
        public override void Update()
        {
        }
        public static void HackPlayer(int sourceId, int targetId)
        {
            RoleSelect.ChangeMainRole(targetId, (int)Roles.MadMate);
        }
        public override void ShowMap(ref MapBehaviour mapBehaviour)
        {
            if (PlayerControl.LocalPlayer.PlayerId == PlayerId)
            {
                //mapBehaviour.ShowSabotageMap();
                //mapBehaviour.countOverlayAllowsMovement = false;
                //mapBehaviour.countOverlay.showLivePlayerPosition =true;
                //mapBehaviour.ColorControl.baseColor = Color.red;
                //var count = mapBehaviour.countOverlay;

            }
        }
    }
}
