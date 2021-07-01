using System;
using GameLogic;
using MapEditor.Recording;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public class EraserTool: MonoBehaviour, IPointerClickHandler, ITool
    {
        private readonly Color _eraseColor = Color.white;
        private ToolMod _toolMod;
        public static event Action<float> TakeSnapshot;

        private void Awake()
        {
            _toolMod = FindObjectOfType<ToolMod>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _toolMod.ResetBoxColors();
            _toolMod.SetBoxColor(transform.parent.gameObject);
            ToolMod.CurrentTool = ToolMod.Tools.Eraser;
            if (GameState.CurrentState == GameState.State.Recording)
            {
                TakeSnapshot?.Invoke(Recorder.SnapshotDelay);
            }
        }

        public Color GetColor()
        {
            return _eraseColor;
        }
    }
}