using System;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Menu.Data;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Ads;
using _Scripts.SharedOverall.UI;
using UnityEngine;
using static _Scripts.SharedOverall.Utility.SceneTransitionManager;

namespace _Scripts.Menu.UI
{
    public class StartLevelButton : MonoBehaviour
    {
        private LevelData _levelData;
        public static LevelData LevelDataToUnlock;
        public static event Action<WarningUI.WarningType> ShowWarning;

        private void Awake()
        {
            _levelData = GetComponentInParent<LevelData>();
        }

        public void StartLevel()
        {
            if (_levelData.ScriptableObject.isLocked)
            {
                LevelDataToUnlock = _levelData;
                ShowWarning?.Invoke(WarningUI.WarningType.UnlockLevel);
                return;
            }
            GameModeManager.isDebug = false;
            GameModeManager.LevelGameMode = GameModeManager.GameMode.Play;
            LevelCreator.ScriptableObject = _levelData.ScriptableObject;
            OpenScene(Scenes.Play);
        }

        public static bool CheckCost()
        {
            return PlayerPrefs.GetInt("Coins", 0) >= LevelDataToUnlock.ScriptableObject.GetCost();
        }
    }
}
