using System;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Playing.Resulting.UI;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.UI;
using _Scripts.SharedOverall.Utility;
using UnityEngine;

namespace _Scripts.Gameplay.Shared.UI
{
    public class MenuAwaiter : MonoBehaviour
    { 
        public static event Action<WarningUI.WarningType> ShowWarning;
        public static event Action CloseUI;

#if UNITY_ANDROID
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (BlurManager.IsBlured())
            {
                CloseUI?.Invoke();
            }
            else if (PickerHandler.IsPickerActive())
            {
                PickerHandler.DisablePicker();
            }
            else
            {
                TryGotoMenu();
            }
        }
#endif
        public void TryGotoMenu()
        {
            if (PlayerPrefs.GetInt("ExitWarning", 0) == 1 || LevelCompleter.IsLevelCompleted)
            {
                GotoMenu();
            }
            else if(LevelCreator.Stage > 0 || GameModeManager.CurrentGameMode == GameModeManager.GameMode.Paint)
            {
                ShowWarning?.Invoke(WarningUI.WarningType.Exit);
            }
            else
            {
                GotoMenu();
            }
        }

        public static void GotoMenu()
        {
            SceneTransitionManager.OpenScene(SceneTransitionManager.Scenes.Menu);
        }
    }
}
