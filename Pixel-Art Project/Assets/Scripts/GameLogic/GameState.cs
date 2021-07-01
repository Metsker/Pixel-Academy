namespace GameLogic
{
    public static class GameState
    {
        public static State CurrentState { get; set; } = State.Gameplay;

        public enum State
        {
            Recording,
            AnimPlaying,
            Gameplay
        }
    }
}