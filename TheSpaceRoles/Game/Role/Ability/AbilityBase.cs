namespace TSR.Game.Role.Ability
{
    public abstract class AbilityBase:IAbilityBase

    {
        public FPlayerControl FPlayerControl{get;private set;}

        public abstract void Init();

        public void SetPlayerControl(FPlayerControl fplayerControl)
        {
            this.FPlayerControl = fplayerControl;
        }   
        //public AbilityParent Parent; <-過去の私がなんか作ったけど何のために作ったかわかんないのでコメントアウト中
    }
    public interface IAbilityBase
    {
        public virtual void Init() { }
        public virtual void OnGameEnd() { }
        public virtual void OnGameStart() { }
        public virtual void OnMeetingEnd() { }
        public virtual void OnMeetingStart() { }
        public virtual void OnPlayerDied() { }
        public virtual void OnPlayerJoined() { }
        public virtual void OnPlayerLeft() { }
        public virtual void OnRoleAssigned() { }
        public virtual void MapClosed() { }
        public virtual void MapOpened(ref MapBehaviour map, ref MapOptions opts) { }

    }
    public abstract class ActionButtonBase : AbilityBase
    {
        public ActionButtonBase() { }
        public ActionButton actionButton;
    }
    public abstract class PassiveSkillBase : AbilityBase
    {
        public PassiveSkillBase() { }
    }
    public abstract class Meeting : AbilityBase
    {
        public Meeting() { }
    }

}
