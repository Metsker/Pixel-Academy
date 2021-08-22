using System.Collections.Generic;
using UnityEngine;

namespace GeneralLogic
{
    public class GameModManager : MonoBehaviour
    {
        public static GameMod CurrentGameMod { get; private set; }
        public static GameMod LevelGameMod { get; set; }
        public static bool Debug { get; private set; }

        [SerializeField] private GameMod gameModInEditor;
        [SerializeField] private bool debugInEditor;
        [Header("Objects")]
        [SerializeField] private List<GameObject> playObjectsToEnable;
        [SerializeField] private List<GameObject> playObjectsToDisable;
        [SerializeField] private List<GameObject> paintObjectsToEnable;
        [SerializeField] private List<GameObject> paintObjectsToDisable;
        [SerializeField] private List<GameObject> recordObjectsToEnable;
        [SerializeField] private List<GameObject> recordObjectsToDisable;

        public enum GameMod
        {
            Play,
            Paint,
            Record
        }

        private void Awake()
        {
            if (!Debug) SwitchMod(LevelGameMod);
        }

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            SwitchMod(gameModInEditor);
            SwitchDebug();
        }

        private void SwitchMod(GameMod gameMod)
        {
            switch (gameMod)
            {
                case GameMod.Play:
                    SwitchState(playObjectsToEnable, playObjectsToDisable, gameMod);
                    break;
                case GameMod.Paint:
                    SwitchState(paintObjectsToEnable, paintObjectsToDisable, gameMod);
                    break;
                case GameMod.Record:
                    SwitchState(recordObjectsToEnable, recordObjectsToDisable, gameMod);
                    break;
            }
        }
        
        private void SwitchState(List<GameObject> toEnable, List<GameObject> toDisable, GameMod gameMod)
        {
            CurrentGameMod = gameMod;
            
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

        private void SwitchDebug()
        {
            if (Debug == debugInEditor) return;
            Debug = debugInEditor;
        }
    }
}