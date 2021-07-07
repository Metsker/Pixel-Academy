using System;
using Gameplay;
using UnityEngine;
using AnimPlaying;
using GameLogic;

namespace MapEditor.PresetSettings
{
    public class DrawingPanelCreatorEditor : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject gridPreset;
        [Tooltip("In GameMod")]
        [SerializeField] private int sideSize;
        public static SizeFieldHandler sizeFieldHandler;
        private int _cashSize;
        public static event Action ToggleUI;

        private void Awake()
        {
            sizeFieldHandler = FindObjectOfType<SizeFieldHandler>();
        }

        public void Create()
        {
            int count = 0;
            switch (GameModManager.CurrentGameMod)
            {
                case GameModManager.GameMod.Gameplay:
                    count = sideSize * sideSize;
                    break;
                case GameModManager.GameMod.Editor:
                    count = sizeFieldHandler.size * sizeFieldHandler.size;
                    break;
            }
            
            if(count == 0 || CheckForExist(count)) return;
            for (var i = 0; i < count; i++)
            {     
                var p = Instantiate(gridPreset, panel.transform);
                 p.name = "Px " + "(" + i + ")";
            }
            _cashSize = count;
            
            LevelTemplateCreator.CreateList();
            if(GameModManager.CurrentGameMod == GameModManager.GameMod.Editor) ToggleUI?.Invoke();
        }

        private bool CheckForExist(int count)
        {
            if (_cashSize == count)
            {
                Debug.LogWarning("Уже выбран");
                return true;
            }
            
            foreach (var pixel in FindObjectsOfType<ClickOnPixel>())
            {
                Destroy(pixel.gameObject);
            }
            return false;
        }
    }
}
