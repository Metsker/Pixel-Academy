using System;
using GameLogic;
using UnityEngine;

namespace MapEditor.PresetSettings
{
    public class DrawingBuilderUI : MonoBehaviour
    {
        [SerializeField] private GameObject createClipUI;
        [SerializeField] private GameObject drawingBuilderUI;
        private bool _panelState;

        private void OnEnable()
        {
            DrawingPanelCreator.ToggleUI += ToggleUI;
        }

        private void OnDisable()
        {
            DrawingPanelCreator.ToggleUI -= ToggleUI;
        }

        private void ToggleUI()
        {
            DisablePanel();
            if (ClipListLoader.AnimationClips.Count > 0) return;
            createClipUI.SetActive(true);
        }
        
        public void DisablePanel()
        {
            drawingBuilderUI.SetActive(_panelState);
            _panelState = !_panelState;
        }
    }
}