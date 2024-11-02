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

        Mayor,

        NiceSwapper,//all
        EvilSwapper,//all
        Sheriff,//c 
        EvilHacker,
        //all or other(ここから　
        //all
        Engineer,//c
        Deputy,//c,m?
        Lighter,//all
        Detective,//all
        TimeMaster,//all, iとかいらねえだろ
        Medic,//all,cだけだろこれぇ 
        Seer,//all
        Hacker,//all
        Tracker,//all
        Snitch,//c,m,? 仕様変更入るけど
        Spy,//c,j?
        Portalmaker,//c ?
        Securityguard,//c
        //Guesser,//all
        Medium,//c
        Trapper,//all

        NiceMini,
        EvilMini,
        NiceGuesser,
        EvilGuesser,

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
    public static class RoleData
    {
        public static List<CustomRole> GetCustomRoles =>
        [

            new Crewmate(),
            new Impostor(),
            new Sheriff(),
            new NiceMini(),
            new Vampire(),
            new SerialKiller(),
            new Madmate(),
            new Jackal(),
            new NiceGuesser(),
            new EvilGuesser(),
            new EvilMini(),
            new Jester(),
            new Mayor(),
            new NiceSwapper(),
            new EvilSwapper(),
            new EvilHacker(),
        ];

        public static List<CustomRole> GetCustomRoles_Normal =>
        [
            new Crewmate(),
            new Impostor(),
            new Madmate(),
            new Jackal(),
            new Jester(),
        ];

        public static List<CustomTeam> GetCustomTeams =>
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
        public static Color GetColorFromTeams(Teams team)
        {

            try
            {

                return GetCustomTeams.First(x => x.Team == team).Color;
            }
            catch
            {

            }
            return Color.magenta;

        }
        public static CustomTeam GetCustomTeamFromTeam(Teams team)
        {
            if (!GetCustomTeams.Any(x => x.Team == team)) { Logger.Error($"{team} is not contained in RoleMasterLink"); return null; }
            return GetCustomTeams.First(x => x.Team == team);


        }
        public static string GetOtherColoredName => Helper.ColoredText(GetOtherRolesColor, Translation.GetString("team.other.name"));

        public static Color GetOtherRolesColor => ColorFromColorcode("#777777");

        public static string GetColoredTeamNameFromTeam(Teams team)
        {
            if (GetCustomTeams.Any(s => s.Team == team))
            {
                return GetCustomTeams.First(x => x.Team == team).ColoredTeamName;
            }
            if ((int)team == -1)
            {
                return ColoredText(Color.magenta, Translation.GetString("team.additional.name"));
            }
            return ColoredText(Color.magenta, Translation.GetString("team." + team.ToString() + ".name"));


        }
        public static string GetColoredRoleNameFromRole(Roles role)
        {
            if (GetCustomRoles.Any(s => s.Role == role))
            {
                return GetCustomRoles.First(x => x.Role == role).ColoredRoleName;
            }
            return ColoredText(Color.magenta, Translation.GetString("team." + role.ToString() + ".name"));


        }

        public static string GetColoredTeamNameFromRole(Roles role) => GetCustomRoleFromRole(role).ColoredRoleName;

        public static CustomRole GetCustomRoleFromRole(Roles roles)
        {
            if (!GetCustomRoles.Any(x => x.Role == roles)) { Logger.Error($"{roles} is not contained in RoleMasterLink"); return null; }
            return GetCustomRoles.First(x => x.Role == roles);
        }
        public static CustomRole GetCustomRole_NormalFromTeam(Teams teams)
        {
            if (!GetCustomRoles_Normal.Any(x => x.team == teams)) { Logger.Error($"{teams} is not contained in RoleMasterNoramlLink"); return null; }

            return GetCustomRoles_Normal.First(x => x.team == teams);
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
