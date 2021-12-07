using _Scripts.GameplayMod.Animating;
using _Scripts.GeneralLogic.Menu.Logic;
using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.UI
{
    public class DrawingStartButton : MonoBehaviour
    {
        public void Select()
        {
            if (SizeStep.XSide == 0 || SizeStep.YSide  == 0) return;
            GameModeManager.isDebug = false;
            GameModeManager.LevelGameMode = GameModeManager.GameMode.Paint;
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            SceneTransitionManager.OpenScene(1);
        }
    }
}
