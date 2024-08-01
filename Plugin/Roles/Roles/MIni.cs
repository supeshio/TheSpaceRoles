using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class Mini : CustomRole
    {
        public int age = 0;
        public float Timer;
        public float MaxTimer = 400;
        public Mini()
        {
            teamsSupported = GetLink.GetAllTeams();
            Role = Roles.Mini;
            Color = ColorFromColorcode("#f5f5f5");
        }
        public override void HudManagerStart(HudManager hudManager)
        {
            //Logger.Info("minimini");
            var pc = PlayerControl.LocalPlayer;
            SetPlayerScale(pc, 0.4f);
            var writer = Rpc.SendRpcUsebility(Rpcs.UseAbility, Role, PlayerId, 0);
            writer.Write(0);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Timer = MaxTimer;
            SetAge(PlayerId, 0);

        }
        public override void Update()
        {

            Timer -= Time.deltaTime;
            if (age >= 20) return;
            if (MaxTimer - Timer > (age + 1) * MaxTimer / 20)
            {
                age += 1;
                var writer = Rpc.SendRpcUsebility(Rpcs.UseAbility, Role, PlayerId, 0);
                writer.Write(age);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                SetAge(PlayerId, age);
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
            var mini = (Mini)DataBase.AllPlayerRoles[playerId].First(x => x.Role == Roles.Mini);
            mini.age = _age;
            DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text = $"{mini.PlayerName}({mini.age})";

            SetPlayerScale(player, 0.4f + _age / 20f * 0.6f);

            if (mini.age >= 20) DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text = $"{mini.PlayerName}";
            Logger.Info($"{_age}");
        }

    }
}
