using System;
using GameLogic;
using Gameplay;
using HSVPicker;
using UnityEngine;

namespace MapEditor.ColorPresets
{
    public class ColorPresetInstanceButton : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject preset;
        private int i;
        public static event Action UpdateRecorder;

        private void Start()
        {
            i = FindObjectsOfType<ColorPreset>().Length;
        }

        public void Create()
        {
            var g = Instantiate(preset, panel.transform);
            g.name = "Preset Color " + "(" + i + ")";
            i++;
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                UpdateRecorder?.Invoke();
            }
        }

        public void Delete()
        {
            if (i <= 0) return;
            i--;
            Destroy(GameObject.Find("Preset Color " + "(" + i + ")"));
        }
    }
}
