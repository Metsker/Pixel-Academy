using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Menu.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Utility;
using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class DrawingStartButton : MonoBehaviour
    {
        public void Select()
        {
            if (SizeStep.XSide == 0 || SizeStep.YSide  == 0) return;
            GameModeManager.isDebug = false;
            GameModeManager.LevelGameMode = GameModeManager.GameMode.Paint;
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            SceneTransitionManager.OpenScene(SceneTransitionManager.Scenes.Play);
        }
    }
}
