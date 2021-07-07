using System;
using GameLogic;
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
            currentColorImg.color = FindObjectOfType<ColorPreset>().GetComponent<Image>().color;
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
        
        public Color GetColor()
        {
            return currentColorImg.color;
        }
    }
}