using System.Collections.Generic;
using TMPro;
using TSR.Game.Role;
using TSR.Game.Role.Ability;
using UnityEngine;

namespace TSR.Game
{
    [SmartPatch]
    public class FPlayerControl
    {
        private static FPlayerControl _LocalPlayer;
        /// <summary>
        /// LocalPlayerのFPC
        /// </summary>
        public static FPlayerControl LocalPlayer => _LocalPlayer;
        public bool IsLocalPlayer => this.PlayerId == LocalPlayer.PlayerId;
        public PlayerControl PC;
        public NetworkedPlayerInfo NPI;
        public RoleBase FRole;
        public TeamBase FTeam;
        public string Role = "tsr:none";
        public string Team = "tsr:none";
        public int PlayerId;
        public string PlayerName;
        public string RoleDescription;
        public string HatId;
        public string SkinId;
        public string VisorId;
        public string NamePlateId;
        public string PetId;
        public uint Level;
        public Color32 RoleColor;
        public Color32 TeamColor;
        public List<AbilityBase> Abilitiies=[];
        public bool RoleAssigned = false;
        public TextMeshPro RoleText;
        public TextMeshPro MeetingRoleText;
        public TextMeshPro PlayerNameText() => this.PC.cosmetics.nameText;
        public TextMeshPro MeetingPlayerNameText()
        { 
            foreach (var k in MeetingHud.Instance.playerStates) 
            {
                if (k.TargetPlayerId == PlayerId)
                {
                    return k.NameText;
                }
            }
            return null;
        }
        public bool Disconnected => PC.Data.Disconnected;
        public bool IsDead => PC.Data.IsDead;
        public Vector2 GetTruePosition() => PC.GetTruePosition();
        public bool AmOwner => PC.AmOwner;
        public bool IsNull => PC == null&&this ==null;
        public Collider2D Collider => PC.Collider;
        public static void AddOrUpdatePlayer(List<FPlayerControl> fpcs)
        {
            _allPlayers = new FPlayerControl[fpcs.Count];
            foreach (var fpc in fpcs)
            {
                _allPlayers[fpc.PlayerId] = fpc;
            }

        }
        public static FPlayerControl get(PlayerControl pc)
        {
            return _allPlayers[pc.PlayerId];
        }

        public static Dictionary<string, TeamBase> FTeams = [];
        private static Dictionary<string, TeamBase> _fTeams = [];

        private static FPlayerControl[] _allPlayers = [];
        /// <summary>
        /// すべてのFPC
        /// </summary>
        public static FPlayerControl[] AllPlayer => [.. _allPlayers];
        public FPlayerControl(PlayerControl playercontrol)
        {
            this.PC = playercontrol;
            SetUp();
            Logger.Info($"FPC : {PlayerName}({PlayerId})");
        }
        public void Init()
        {

            var playercontrol = this.PC;
            this.NPI = playercontrol.Data;
            this.Level = playercontrol.Data.PlayerLevel;
            this.PlayerId = playercontrol.PlayerId;
            this.HatId = playercontrol.CurrentOutfit.HatId;
            this.SkinId = playercontrol.CurrentOutfit.SkinId;
            this.VisorId = playercontrol.CurrentOutfit.VisorId;
            this.PetId = playercontrol.CurrentOutfit.PetId;
            this.NamePlateId = playercontrol.CurrentOutfit.NamePlateId;
            this.PlayerName = playercontrol.CurrentOutfit.PlayerName;
            if (playercontrol.PlayerId == PlayerControl.LocalPlayer.PlayerId)
            {
                _LocalPlayer = this;
            }
        }
        public void SetRole(RoleBase role)
        {
            Logger.Error($"SetRole : {role.RoleName}");
            this.FRole = role;
            role.SetPlayer(this);
            this.Role = role.Role();
            this.RoleDescription = role.RoleDescription;
            this.RoleColor = role.RoleColor();
            this.Team = role.Team();
            this.FTeam =TeamBase.GetTeamBase(role.Team());
            this.RoleAssigned = true;
            NewRoleName(this.FRole.RoleName, this.FRole.RoleColor(), this.FTeam.TeamColor());
            //ここで初期化するのでいいのだろうか?
            //TODO:初期化場所
            this.FRole.InitAbilityAndButton();
            if (this.IsLocalPlayer)
            {
                this.FRole.CreateButtons();
                FGameManager.Manager.Local.Target = role.TargetBase();
                FGameManager.Manager.Local.Highlight = new(role.TargetColor());
            }
        }
        public void NewRoleName(string RoleText,Color32 RoleColor,Color32 TeamColor)
        {
            Logger.Info($"Set RoleName : {RoleText}");
            this.PlayerNameText().color = TeamColor;
            if(this.RoleText==null)
            {
                this.RoleText = GameObject.Instantiate(this.PlayerNameText().gameObject).GetComponent<TextMeshPro>();
                this.RoleText.name = "RoleText";
                this.RoleText.transform.SetParent(this.PlayerNameText().transform.parent);
                this.RoleText.transform.localPosition = new Vector3(0, 0.3f, 0);
                this.RoleText.fontSize = 1.2f;
            }
            this.RoleText.color = RoleColor;
            this.RoleText.text = RoleText;
            if(MeetingRoleText!=null)
            {
                this.MeetingRoleText?.color = RoleColor;
                this.MeetingRoleText?.text = RoleText;
                this.MeetingPlayerNameText()?.color = TeamColor;
            }
        }
        public static void MeetingSetUp(MeetingHud meeting)
        {
            foreach (var p in meeting.playerStates)
            {
                var fpc = FPlayerControl.AllPlayer[p.TargetPlayerId];
                fpc.MeetingRoleText = GameObject.Instantiate(p.NameText.gameObject).GetComponent<TextMeshPro>();
                fpc.MeetingRoleText.transform.SetParent(p.NameText.transform.parent);
                fpc.MeetingRoleText.name = "RoleText";
                fpc.MeetingRoleText.transform.localPosition = new Vector3(0.3384f, 0.2f, 0);
                fpc.MeetingRoleText.fontSize = 1.4f;
                fpc.NewRoleName(fpc.FRole.RoleName, fpc.FRole.RoleColor(), fpc.FTeam.TeamColor());
            }
        }
        public void SetUp()
        {

            if (PC.Data.NetId == PlayerControl.LocalPlayer.Data.NetId)
            {
                _LocalPlayer = this;
            }
            Init();
        }
        public void ToggleHighlight(bool active, Color32? color)
        {
            if (active && color.HasValue)
            {
                //Logger.Info($"color:{color.Value.rgba}");

                // Color32 → Color
                Color unityColor = color.Value;

                // Il2CppSystem.Nullable<Color> に変換
                var il2cppColor = new Il2CppSystem.Nullable<Color>(unityColor);

                this.PC.cosmetics.SetOutline(true, il2cppColor);
            }
            else
            {
                //Logger.Info($"color:null {PlayerId}");
                // nullの代わりに default を渡す
                this.PC.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>());
            }
        }

        public bool IsValidTarget()
        {
            var target = this;
            return !(target == null) && !target.Disconnected && !target.IsDead && !(target.Role == null) && !(target.NPI == null) && !target.PC.inVent && !target.PC.inMovingPlat && target.PC.Visible;
        }

        public void GameStartReset()
        {
            FPlayerControl fpc = this;
            fpc.FTeam.Init();
            fpc.FRole.Init();
            foreach (var abilitiy in fpc.Abilitiies)
            {
                abilitiy.Init();
            }
        }
        //ここからrpc関連かも
        public void RpcMurder(FPlayerControl Target,bool didSucceed)
        {
            PC.RpcMurderPlayer(Target.PC,didSucceed);
        }


        [SmartPatch]
        public static class FPCSet
        {

            [SmartPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGame))]
            public static class Start
            {
                public static void Prefix(AmongUsClient __instance)
                {
                    if (FGameManager.Manager == null)
                    {
                        Logger.Error("fgm is not created");
                    }
                    FPCSET();
                }
            }
            public static void FPCSET()
            {
                Logger.Info("FPC SET");
                List<FPlayerControl> fs = [];
                foreach (var pc in PlayerControl.AllPlayerControls)
                {
                    fs.Add(new FPlayerControl(pc));
                }


                FPlayerControl.AddOrUpdatePlayer(fs);
            }
            [SmartPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerLeft))]
            public static class End
            {
                public static void Prefix(AmongUsClient __instance, InnerNet.ClientData data, DisconnectReasons reason)
                {
                    //FPlayerControl.DeletePlayer(data.Character.PlayerId);
                }
            }
        }
    }
}
