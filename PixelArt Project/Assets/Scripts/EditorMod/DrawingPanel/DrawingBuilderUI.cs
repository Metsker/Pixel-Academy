using System;
using EditorMod.Animating;
using GeneralLogic.DrawingPanel;
using UnityEngine;

namespace EditorMod.DrawingPanel
{
    public class DrawingBuilderUI : MonoBehaviour
    {
        public GameObject drawingBuilderUI;
        private bool _panelState;

        private void OnEnable()
        {
            DrawingPanelCreator.ToggleUI += TogglePanel;
            ClipCreator.UpdateUI += DisablePanel;
        }

        private void OnDisable()
        {
            DrawingPanelCreator.ToggleUI -= TogglePanel;
            ClipCreator.UpdateUI -= DisablePanel;
        }

        public void TogglePanel()
        {
            _panelState = !drawingBuilderUI.activeSelf;
            drawingBuilderUI.SetActive(_panelState);
        }

        public void DisablePanel()
        {
            if (!drawingBuilderUI.activeSelf) return;
            drawingBuilderUI.SetActive(false);
        }
    }
}