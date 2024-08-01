using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    /*all すべてのteam

        c crew
        i impostor
        m madmate
        j jackal
        je jester
        ar arsonist
        vu vulture

        */
    public enum Roles : int
    {
        //normal(これが当てはまることはおそらくないかと
        None = 0,
        //default
        Crewmate,
        Impostor,
        MadMate,
        Jackal,
        Jester,
        Arsonist,
        Vulture,
        lawyer,//弁護士
        Prosecutor,//検察官?
        Pursuer,//追跡者?
        Thief,//泥棒?
        Vampire,//ヴァンパイア
        SerialKiller,//シリアルキラー



        //all or other(ここから　
        Mayor,//all
        Engineer,//c
        Sheriff,//c 
        Deputy,//c,m?
        Lighter,//all
        Detective,//all
        TimeMaster,//all, iとかいらねえだろ
        Medic,//all,cだけだろこれぇ 
        Swapper,//all
        Seer,//all
        Hacker,//all
        Tracker,//all
        Snitch,//c,m,? 仕様変更入るけど
        Spy,//c,j?
        Portalmaker,//c ?
        Securityguard,//c
        Guesser,//all
        Medium,//c
        Trapper,//all

        Mini,

    }
    public enum Teams : int
    {
        None = 0,
        Crewmate,
        Madmate,
        Impostor,
        Jackal,
        //Oppotunist,
        Jester,
        Arsonist,
        Vulture,
        lawyer,//弁護士
        Prosecutor,//検察官?
        Pursuer,//追跡者?
        Thief//泥棒?
    }
    public enum CustomGameOverReason : int
    {
        GhostTown,
        Jester,
    }

    /// <summary>
    /// Linkだよ!!
    /// </summary>
    public static class GetLink
    {
        public static List<CustomRole> CustomRoleLink =>
        [

            new Crewmate(),
            new Impostor(),
            new Sheriff(),
            new Mini(),
            new Vampire(),
            new SerialKiller(),
            new Madmate(),
            new Jackal(),
            new Guesser(),
        ];

        public static List<CustomRole> CustomRoleNormalLink =>
        [
            new Crewmate(),
            new Impostor(),
            new Madmate(),
            new Jackal(),
        ];

        public static List<CustomTeam> CustomTeamLink =>
        [
            new CrewmateTeam(),
            new ImpostorTeam(),
            new JackalTeam(),
            new JesterTeam(),
            new MadmateTeam(),

        ];





        /*public static Dictionary<Teams, Color> ColorFromTeams = new()
        {
            {Teams.Crewmate,Palette.CrewmateBlue},
            {Teams.Impostor, Palette.ImpostorRed },
            {Teams.Madmate,ColorFromColorcode("#aa1010")},
            {Teams.Jackal,ColorFromColorcode("#09afff") },
            {Teams.Jester, ColorFromColorcode("#ea618e") },
        };*/
        public static Color ColorFromTeams(Teams team)
        {

            try
            {

                return CustomTeamLink.First(x => x.Team == team).Color;
            }
            catch
            {

            }
            return Color.magenta;

        }
        public static CustomTeam GetCustomTeam(Teams team)
        {
            if (!CustomTeamLink.Any(x => x.Team == team)) { Logger.Error($"{team} is not contained in RoleMasterLink"); return null; }
            return CustomTeamLink.First(x => x.Team == team);


        }
        public static string GetOtherColoredName => Helper.ColoredText(GetOtherRolesColor, Translation.GetString("team.other.name"));

        public static Color GetOtherRolesColor => ColorFromColorcode("#777777");

        public static string GetColoredTeamName(Teams team)
        {
            if (CustomTeamLink.Any(s => s.Team == team))
            {
                return CustomTeamLink.First(x => x.Team == team).ColoredTeamName;
            }
            if ((int)team == -1)
            {
                return ColoredText(Color.magenta, Translation.GetString("team.additional.name"));
            }
            return ColoredText(Color.magenta, Translation.GetString("team." + team.ToString() + ".name"));


        }
        public static string GetColoredRoleName(Roles role)
        {
            if (CustomRoleLink.Any(s => s.Role == role))
            {
                return CustomRoleLink.First(x => x.Role == role).ColoredRoleName;
            }
            return ColoredText(Color.magenta, Translation.GetString("team." + role.ToString() + ".name"));


        }

        public static string GetColoredTeamName(Roles role) => GetCustomRole(role).ColoredRoleName;

        public static CustomRole GetCustomRole(Roles roles)
        {
            if (!CustomRoleLink.Any(x => x.Role == roles)) { Logger.Error($"{roles} is not contained in RoleMasterLink"); return null; }
            return CustomRoleLink.First(x => x.Role == roles);
        }
        public static CustomRole GetCustomRoleNormal(Teams teams)
        {
            if (!CustomRoleNormalLink.Any(x => x.teamsSupported.Contains(teams))) { Logger.Error($"{teams} is not contained in RoleMasterNoramlLink"); return null; }

            return CustomRoleNormalLink.First(x => x.teamsSupported.Contains(teams));
        }
        public static Teams[] GetAllTeams()
        {
            var teams = new List<Teams>();
            foreach (Teams team in Enum.GetValues(typeof(Teams)))
            {
                teams.Add(team);

            }
            return [.. teams];
        }

    }
}
