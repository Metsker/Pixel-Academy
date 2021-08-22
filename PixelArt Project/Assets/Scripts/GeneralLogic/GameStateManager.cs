﻿namespace GeneralLogic
{
    public static class GameStateManager
    {
        public static GameState CurrentGameState { get; set; } = GameState.Drawing;

        public enum GameState
        {
            Drawing,
            Animating,
            Recording
        }
    }
}