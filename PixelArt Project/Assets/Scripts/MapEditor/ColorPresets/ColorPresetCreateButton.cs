using System;
using GameLogic;
using HSVPicker;
using UnityEngine;

namespace MapEditor.ColorPresets
{
    public class ColorPresetCreateButton : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject preset;
        public ColorPicker picker;
        private int _i;
        public static event Action UpdateRecorder;

        private void Start()
        {
            _i = FindObjectsOfType<ColorPreset>().Length;
        }

        public void Create()
        {
            var c = Instantiate(preset, panel.transform);
            c.name = "Preset Color " + "(" + _i + ")";
            _i++;
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                UpdateRecorder?.Invoke();
            }
        }
    }
}
