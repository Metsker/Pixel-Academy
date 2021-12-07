using System;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Shared.UI
{
    public class MenuButton : BaseTool
    { 
        public static event Action<WarningUI.WarningType> ShowWarning;
        public static event Action CloseWarning;
        public static event Func<bool> IsWarningActive; 
        
#if UNITY_ANDROID
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (PickerHandler.IsPickerActive())
            {
                PickerHandler.DisablePicker();
            }
            else
            {
                GoToMenuWithWarning();
            }
        }
#endif
        public void GotoMenu()
        {
            SceneTransitionManager.OpenScene(0);
        }
        
        public void GotoMenuWithAnimation()
        {
            ClickEventNoStates();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play && 
                GameStateManager.CurrentGameState == GameStateManager.GameState.Animating && LevelCreator.Stage == 0)
            {
                GotoMenu();
            }
            else
            {
                GoToMenuWithWarning();
            }
        }
        private void GoToMenuWithWarning()
        {
            if (IsWarningActive != null && IsWarningActive.Invoke())
            {
                CloseWarning?.Invoke();
            }
            else if (PlayerPrefs.GetInt("ExitWarning", 0) == 1)
            {
                GotoMenu();
            }
            else
            {
                ShowWarning?.Invoke(WarningUI.WarningType.Exit);
            }
        }
    }
}
