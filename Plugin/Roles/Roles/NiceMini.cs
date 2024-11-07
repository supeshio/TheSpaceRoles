using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class NiceMini : CustomRole
    {
        public int age = 0;
        public float Timer;
        public float MaxTimer = 400;
        public NiceMini()
        {
            Team = Teams.Crewmate;
            Role = Roles.NiceMini;
            Color = ColorFromColorcode("#ffeb57");
        }
        public override void HudManagerStart(HudManager hudManager)
        {
            var pc = PlayerControl.LocalPlayer;
            SetPlayerScale(pc, 0.4f);
            var writer = CustomRPC.SendRpcUsebility(Role, PlayerId, 0);
            writer.Write(0);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Timer = MaxTimer;
            SetAge(PlayerId, 0);

        }
        public override void Update()
        {
            if (PlayerId == PlayerControl.LocalPlayer.PlayerId)
            {

                Timer -= Time.deltaTime;
                if (age >= 20) return;
                if (MaxTimer - Timer > (age + 1) * MaxTimer / 20)
                {
                    age += 1;
                    var writer = CustomRPC.SendRpcUsebility(Role, PlayerId, 0);
                    writer.Write(age);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    SetAge(PlayerId, age);
                }
            }
        }
        public override void APUpdate()
        {
            if (age >= 20)
            {

                DataBase.AllPlayerControls().First(x => x.PlayerId == PlayerId).cosmetics.nameText.text = $"{PlayerName}";

            }
            else
            {
                DataBase.AllPlayerControls().First(x => x.PlayerId == PlayerId).cosmetics.nameText.text = $"{PlayerName}({age})";

                SetPlayerScale(DataBase.AllPlayerControls().First(x => x.PlayerId == PlayerId), 0.4f + age / 20f * 0.6f);
            }



        }
        public static void SetAge(int playerId, int _age)
        {
            var player = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId);
            var mini = (NiceMini)DataBase.AllPlayerRoles[playerId];
            mini.age = _age;
            DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text = $"{mini.PlayerName}({mini.age})";

            SetPlayerScale(player, 0.4f + _age / 20f * 0.6f);

            if (mini.age >= 20) DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text = $"{mini.PlayerName}";
            Logger.Info($"{_age}");
        }

    }
}
