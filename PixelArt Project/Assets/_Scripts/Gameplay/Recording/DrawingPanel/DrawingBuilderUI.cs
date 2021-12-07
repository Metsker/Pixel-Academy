#if (UNITY_EDITOR)
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.SharedOverall.DrawingPanel;
using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Recording.DrawingPanel
{
    public class DrawingBuilderUI : MonoBehaviour
    {
        public GameObject drawingBuildUI;
        public TMP_InputField[] fields;

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
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) 
            {
                if (fields.Length <= FieldSizeHandler.selectedFieldIndex)
                {
                    FieldSizeHandler.selectedFieldIndex = 0;
                }
                fields[FieldSizeHandler.selectedFieldIndex].Select();
                FieldSizeHandler.selectedFieldIndex++;
            }
        }

        public void TogglePanel()
        {
            drawingBuildUI.SetActive(!drawingBuildUI.activeSelf);
        }

        public void DisablePanel()
        {
            if (!drawingBuildUI.activeSelf) return;
            drawingBuildUI.SetActive(false);
        }
    }
}
#endif