using System;
using GameLogic;
using UnityEngine;

namespace MapEditor.PresetSettings
{
    public class UIDrawingPanel : MonoBehaviour
    {
        [SerializeField] private GameObject createClipUI;
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
            if (ClipListLoader.AnimationClips.Count == 0)
            {
                createClipUI.SetActive(true);
            }
            DisablePanel();
        }
        
        public void DisablePanel()
        {
            transform.parent.gameObject.SetActive(_panelState);
            _panelState = !_panelState;
        }
    }
}