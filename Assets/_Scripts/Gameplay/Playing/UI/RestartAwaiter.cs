using System;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.UI;
using _Scripts.SharedOverall.Utility;
using UnityEngine;

namespace _Scripts.Gameplay.Playing.UI
{
    public class RestartAwaiter : MonoBehaviour
    {
        public static event Action<WarningUI.WarningType> ShowWarning;
        
        public void RestartWithAnimation()
        {
            if (PlayerPrefs.GetInt("RestartWarning", 0) == 1)
            {
                Restart();
            }
            else if (LevelCreator.Stage > 0 || GameModeManager.CurrentGameMode == GameModeManager.GameMode.Paint)
            {
                ShowWarning?.Invoke(WarningUI.WarningType.Restart);
            }
            else
            {
                Restart();
            }
        }
        public static void Restart()
        {
            SceneTransitionManager.OpenScene(SceneTransitionManager.Scenes.Play);
        }
    }
}
