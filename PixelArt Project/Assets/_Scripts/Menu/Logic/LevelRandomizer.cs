using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Utility;
using UnityEngine;
using static _Scripts.SharedOverall.LevelGroupsLoader;

namespace _Scripts.Menu.Logic
{
    public class LevelRandomizer : MonoBehaviour
    {
        public void OnClick()
        {
            GameModeManager.isDebug = false;
            GameModeManager.LevelGameMode = GameModeManager.GameMode.Play;
            var levelGroups = levelGroupsLoader.levelGroups;
            
            int rGroup;
            int rLevel;
            do
            {
                rGroup = Random.Range(0, levelGroups.Count);
                rLevel = Random.Range(0, levelGroups[rGroup].levels.Count);
            } 
            while (levelGroups[rGroup].levels[rLevel].isLocked || 
                   DifficultyFilterManager.currentDifficulty != DifficultyFilterManager.Difficulties.None 
                   && levelGroups[rGroup].levels[rLevel].difficulty != DifficultyFilterManager.currentDifficulty);

            LevelCreator.ScriptableObject = levelGroups[rGroup].levels[rLevel];
            SceneTransitionManager.OpenScene(SceneTransitionManager.Scenes.Play);
        }
    }
}
