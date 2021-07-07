using System;
using GameLogic;
using MapEditor.ColorPresets;
using MapEditor.Recording;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay
{
    public class ClickOnPixel : MonoBehaviour, IPointerClickHandler
    {
        private PencilTool _pencil;
        private EraserTool _eraser;
        private Image _image;
        public static bool firstSnapshotBool { get; set; }
        public static Color firstPixelColor { get; private set; }
        public static event Action<float> TakeSnapshot;
        private void Awake()
        {
            _image = GetComponent<Image>();
            _pencil = FindObjectOfType<PencilTool>();
            _eraser = FindObjectOfType<EraserTool>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (ToolMod.CurrentTool)
            {
                case ToolMod.Tools.Pencil:
                    OnPointer(_pencil.GetColor());
                    break;
                case ToolMod.Tools.Eraser:
                    OnPointer(_eraser.GetColor());
                    break;
            }
        }

        private void OnPointer(Color c)
        {
            if (_image.color == c)
            {
                Debug.LogWarning("Цвета совпадают");
                return;
            }
            if (ColorPreset.AnimCount > 0)
            {
                Debug.LogWarning("Дождись окончания анимации");
                return;
            }

            _image.color = c;
            
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                TakeSnapshot?.Invoke(Recorder.SnapshotDelay);
                if (firstSnapshotBool) return;
                firstSnapshotBool = true;
                firstPixelColor = c;
            }
        }
    }
}
