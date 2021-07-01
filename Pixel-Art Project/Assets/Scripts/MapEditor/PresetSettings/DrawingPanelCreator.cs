using Gameplay;
using UnityEngine;
using AnimPlaying;

namespace MapEditor.PresetSettings
{
    public class DrawingPanelCreator : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject gridPreset;
        [SerializeField] private GameObject createClipUI;
        public static DrawingSizeField DrawingSizeField;
        private int _cashSize;
        private bool _panelState;

        private void Awake()
        {
            DrawingSizeField = FindObjectOfType<DrawingSizeField>();
        }

        public void Create()
        {
            if (FindObjectOfType<ClickOnPixel>())
            {
                if (_cashSize == DrawingSizeField.size)
                {
                    Debug.LogWarning("Уже выбран");
                    return;
                }
                foreach (var pixel in FindObjectsOfType<ClickOnPixel>())
                {
                    Destroy(pixel.gameObject);
                }
            }
            
            var count = DrawingSizeField.size * DrawingSizeField.size;
            if(count == 0) return;

            for (var i = 0; i < count; i++)
            {     
                var a = Instantiate(gridPreset, panel.transform);
                 a.name = "Px " + i;
            }

            _cashSize = DrawingSizeField.size;
            LevelListManager.CreateList();
            
            if (AnimClipLoader.AnimationClips.Count == 0)
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
