using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.SharedOverall
{
    public class GameModeManager : MonoBehaviour
    {
        public static GameMode CurrentGameMode { get; private set; }
        public static GameMode LevelGameMode { get; set; }
        public static bool isDebug { get; set; } = true; //Editor

        [SerializeField] private GameMode gameModeInEditor;
        [Header("Objects")]
        [SerializeField] private List<GameObject> playObjectsToEnable;
        [SerializeField] private List<GameObject> playObjectsToDisable;
        [SerializeField] private List<GameObject> paintObjectsToEnable;
        [SerializeField] private List<GameObject> paintObjectsToDisable;
        [SerializeField] private List<GameObject> recordObjectsToEnable;
        [SerializeField] private List<GameObject> recordObjectsToDisable;

        public enum GameMode
        {
            Play,
            Paint,
            Record
        }

        private void Awake()
        {
            if (!isDebug) SwitchMod(LevelGameMode);
        }

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            SwitchMod(gameModeInEditor);
        }

        private void SwitchMod(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Play:
                    SwitchState(playObjectsToEnable, playObjectsToDisable, gameMode);
                    break;
                case GameMode.Paint:
                    SwitchState(paintObjectsToEnable, paintObjectsToDisable, gameMode);
                    break;
                case GameMode.Record:
                    SwitchState(recordObjectsToEnable, recordObjectsToDisable, gameMode);
                    break;
            }
        }
        
        private void SwitchState(List<GameObject> toEnable, List<GameObject> toDisable, GameMode gameMode)
        {
            CurrentGameMode = gameMode;
            
            foreach (var v in toDisable)
            {
                if (v.activeSelf == true) 
                    v.SetActive(false);
            }
            foreach (var v in toEnable)
            {
                if (v.activeSelf == false) 
                    v.SetActive(true);
            }
        }
    }
}