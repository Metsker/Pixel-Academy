using System;
using System.Collections;
using _Scripts.Gameplay.Playing.UI;
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

        private static bool _isHintShown;

        public static event Action<TextHint.HintType, float> ShowHint;
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
            ShowHint?.Invoke(TextHint.HintType.Drawing, 3);
            _isHintShown = true;

        }
    }
}
