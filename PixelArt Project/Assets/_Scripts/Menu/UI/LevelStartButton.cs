using System;
using _Scripts.GameplayMod.Animating;
using _Scripts.GameplayMod.Creating;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.Ads;
using _Scripts.GeneralLogic.Menu.Data;
using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.UI
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
