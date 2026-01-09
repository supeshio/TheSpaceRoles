using System;
using System.Collections.Generic;
using TSR.Game.Role.Ability;
using UnityEngine;

namespace TSR.Game.Role
{
#nullable enable
    public abstract class RoleBase
    {
        public enum TargetHighLightType
        {
            None,
            All,//範囲内全員
            MainTarget,//範囲メイン一人[0]
            SubTarget,//範囲サブ一人[1]
        }
        // プロパティ
        public abstract string Role();
        public abstract string Team();
        public FPlayerControl FPlayer { get;private set; }
        public void SetPlayer(FPlayerControl fPlayerControl)
        {
            this.FPlayer = fPlayerControl;
        }
        public string RoleName => Translation.Get($"role.{Role().ToString()}.name"); // デフォルト実装として Role 列挙型の名前を返す
        public string RoleDescription => Translation.Get($"role.{Role().ToString()}.description"); // デフォルトの説明
        public string RoleIntro => Translation.Get($"role.{Role().ToString()}.intro"); // イントロ
        public abstract Color32 RoleColor(); // デフォルトの色
        public abstract void Init();
        public abstract void InitAbilityAndButton();
        public abstract AbilityRange GetAbilityDistance();
        //{
            //return new AbilityRange(AbilityRange.Range.None); 
        //} // デフォルトの能力範囲
        public virtual bool DoTargetHighlight() => false;
        public virtual void OnGameStart() { }
        public virtual void OnGameEnd() { }
        public virtual void OnMeetingStart() { }
        public virtual void OnMeetingEnd() { }
        public virtual void OnPlayerJoined() { }
        public virtual void OnPlayerLeft() { }
        public virtual void OnRoleAssigned() { }
        public virtual void OnPlayerDied() { }
        public void SetAbility(AbilityBase ability)
        {
            ability.SetPlayerControl(FPlayer);
            Abilities.Add(ability);
        }

        public List<AbilityBase> Abilities = [];
        public void AddAbility(AbilityBase ability)
        {
            ability.SetPlayerControl(FPlayer);
            Abilities.Add(ability);
        }
        public List<CustomButton> CustomButtons = [];
        public void AddCustomButton(CustomButton customButton)
        {
            CustomButtons.Add(customButton);
        }
        public void CreateButtons()
        {
            Logger.Info($"CreateButtons");
            foreach (var button in CustomButtons)
            {
                button.CreateButton();

                Logger.Info($"Button : [Text : {button.ButtonText()}]");
            }
        }
        public static RoleBase GetRoleBase(string roleId) => RoleBaseRegister.GetRoleBase(roleId);
        public virtual Func<Color> TargetColor() => ()=> RoleColor();
        public abstract PlayerTargetBase? TargetBase();
        public void Update()
        {

        }

    }
}
