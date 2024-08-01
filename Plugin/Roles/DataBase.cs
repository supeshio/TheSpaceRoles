using System.Collections.Generic;
using System.Linq;

namespace TheSpaceRoles
{
    public static class DataBase
    {
        /// <summary>
        /// playerId,RoleMaster型で役職の型を入れれる
        /// </summary>
        public static Dictionary<int, CustomRole[]> AllPlayerRoles = [];//playerId,roles

        /// <summary>
        /// playerId,Teams型で陣営型を入れれる
        /// </summary>
        public static Dictionary<int, Teams> AllPlayerTeams = [];//playerId,Teams
        public static Dictionary<int, DeathReason> AllPlayerDeathReasons = [];

        public static List<CustomButton> buttons = [];
        public static PlayerControl[] AllPlayerControls()
        {
            return PlayerControl.AllPlayerControls.ToArray().Where(x => !x.isDummy).ToArray();
        }
        /// <summary>
        /// VoteAreaのすべてのプレイヤー
        /// </summary>
        /// <returns>nullの可能性あり</returns>
        public static PlayerVoteArea[] AllPlayerMeeting()
        {
            return MeetingHud.Instance.playerStates.ToArray();
        }
        /// <summary>
        /// RESET!!!!!!
        /// </summary>


        public static void ResetAndPrepare()
        {

            AllPlayerTeams.Clear();
            AllPlayerRoles.Clear();
            AllPlayerDeathReasons.Clear();
            buttons.Clear();

            HudManagerGame.IsGameStarting = false;

            HudManagerGame.OnGameStarted = true;
        }
    }

}
