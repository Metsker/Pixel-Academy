using System;
using System.Collections;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Menu.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEngine;
using static _Scripts.Gameplay.Playing.UI.TextHintInvoker;

namespace _Scripts.Gameplay.Release.Drawing
{
    public class StartDrawing : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DrawingPanelCreator drawingPanelCreator;

        private static bool _isHintShown;
        
        private IEnumerator Start()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Paint) yield break;
            if ((SizeStep.XSide == 0 || SizeStep.YSide == 0) && GameModeManager.isDebug)
            {
                SizeStep.XSide = 8;
                SizeStep.YSide = 8;
            }
            yield return drawingPanelCreator.Create();
            if (_isHintShown) yield break;
            TextHintInvoker.Invoke(HintType.Drawing, 3);
            _isHintShown = true;
        }
    }
}
