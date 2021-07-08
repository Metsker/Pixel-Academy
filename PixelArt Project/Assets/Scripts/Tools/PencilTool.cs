using System;
using GameLogic;
using Gameplay;
using MapEditor.ColorPresets;
using MapEditor.Recording;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools
{
    public class PencilTool : MonoBehaviour, IPointerClickHandler, ITool
    {
        [SerializeField] private Image currentColorImg;
        private ToolMod _toolMod;
        public static event Action<float> TakeSnapshot;

        private void Awake()
        {
            _toolMod = FindObjectOfType<ToolMod>();
        }

        private void Start()
        {
            SetColor(GameModManager.CurrentGameMod);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _toolMod.ResetBoxColors();
            _toolMod.SetBoxColor(transform.parent.gameObject);
            ToolMod.CurrentTool = ToolMod.Tools.Pencil;
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                TakeSnapshot?.Invoke(Recorder.SnapshotDelay);
            }
        }

        public void SetColor(Color c)
        {
            currentColorImg.color = c;
        }
        
        public void SetColor(GameModManager.GameMod mod)
        {
            switch (mod)
            {
                case GameModManager.GameMod.Editor:
                    currentColorImg.color = FindObjectOfType<ColorPreset>().GetComponent<Image>().color;
                    break;
                case GameModManager.GameMod.Gameplay:
                    currentColorImg.color = FindObjectOfType<ResultComparer>().scriptableObject.firstColor;
                    break;
            }
        }
        
        public Color GetColor()
        {
            return currentColorImg.color;
        }
    }
}