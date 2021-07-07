using System;
using UnityEngine;

namespace GameLogic
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