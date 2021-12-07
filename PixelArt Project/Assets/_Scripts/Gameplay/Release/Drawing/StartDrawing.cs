using System;
using System.Collections;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Menu.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Drawing
{
    public class StartDrawing : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DrawingPanelCreator drawingPanelCreator;
        public static event Action<WarningUI.WarningType> ShowTip;

        private IEnumerator Start()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Paint) yield break;
            if ((SizeStep.XSide == 0 || SizeStep.YSide == 0) && GameModeManager.isDebug)
            {
                SizeStep.XSide = 8;
                SizeStep.YSide = 8;
            }
            yield return drawingPanelCreator.Create();
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            
            if (PlayerPrefs.GetInt("DrawingTipWarning", 0) == 1) yield break;
            
            ShowTip?.Invoke(WarningUI.WarningType.DrawingTip1);
        }
    }
}
