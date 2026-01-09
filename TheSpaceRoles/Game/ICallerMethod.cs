namespace TSR.Game
{
    public interface ICallerMethod
    {
        public void Init();

        public void OnGameStart();

        public void OnGameEnd();

        public void OnMeetingStart();

        public void OnMeetingEnd();

        public void OnPlayerJoined();

        public void OnPlayerLeft();

        public void OnRoleAssigned();

        public void OnPlayerDied();
    }
}