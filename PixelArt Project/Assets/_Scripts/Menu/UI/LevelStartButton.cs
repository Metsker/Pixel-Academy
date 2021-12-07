using System;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Menu.Data;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Ads;
using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class LevelStartButton : MonoBehaviour
    {
        private LevelData _levelData;
        public static event Action<WarningUI.WarningType> ShowWarning;

        private void Awake()
        {
            _levelData = GetComponentInParent<LevelData>();
        }

        public void StartLevel()
        {
            if (_levelData.scriptableObject.isLocked)
            {
                LevelAdVideo.LevelDataReference = _levelData;
                ShowWarning?.Invoke(WarningUI.WarningType.UnlockLevel);
                return;
            }
            GameModeManager.isDebug = false;
            GameModeManager.LevelGameMode = GameModeManager.GameMode.Play;
            LevelCreator.scriptableObject = _levelData.scriptableObject;
            LevelCreator.groupScriptableObject = _levelData.groupScriptableObject;
            SceneTransitionManager.OpenScene(1);
        }
    }
}
