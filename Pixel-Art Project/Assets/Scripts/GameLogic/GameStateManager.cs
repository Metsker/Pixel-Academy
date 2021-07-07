using System;
using UnityEngine;

namespace GameLogic
{
    public class GameState : MonoBehaviour
    {
        [SerializeField] private State stateSetter;
        public static State CurrentState { get; set; }

        private void Awake()
        {
            CurrentState = stateSetter;
        }

        public enum State
        {
            Gameplay,
            Editor,
            Recording,
            AnimPlaying
        }
    }
}