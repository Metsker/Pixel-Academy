using System;
using EditorMod.Recording;
using GameplayMod;
using GeneralLogic.Animating;
using GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Logic
{
    public class ClickOnPixel : MonoBehaviour, IPointerClickHandler
    {
        private GameStateToggler _gameStateToggler;
        private Image _image;
        public static event Action<float> TakeSnapshot;
        public static event Func<bool> CheckPicker;
        private void Awake()
        {
            _image = GetComponent<Image>();
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play) 
                _gameStateToggler = FindObjectOfType<GameStateToggler>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (ToolsManager.CurrentTool)
            {
                case ToolsManager.Tools.Pencil:
                    OnPointer(PencilTool.GetColor(), c=> _image.color = c);
                    break;
                case ToolsManager.Tools.Eraser:
                    OnPointer(EraserTool.GetColor(),c=> _image.color = c);
                    break;
                case ToolsManager.Tools.Filler:
                    var targetColor = _image.color;
                    OnPointer(PencilTool.GetColor(), c => OnFiller(c, targetColor));
                    break;
            }
        }

        private void OnPointer(Color c, Action<Color> action) 
        {
            switch (GameModManager.CurrentGameMod)
            {
                case GameModManager.GameMod.Play when !GameStateToggler.isGameStarted:
                    _gameStateToggler.StopClip();
                    return;
                case GameModManager.GameMod.Paint when CheckPicker != null && CheckPicker.Invoke():
                    return;
                case GameModManager.GameMod.Record when CheckPicker != null && CheckPicker.Invoke():
                    return;
            }
            
            if (_image.color == c) return;
            switch (GameStateManager.CurrentGameState)
            {
                case GameStateManager.GameState.Recording when ToolAnimation.isAnyAnimPlaying:
                    Debug.LogWarning("Дождись окончания анимации");
                    return;
                case GameStateManager.GameState.Recording:
                    action(c);
                    TakeSnapshot?.Invoke(Recorder.SnapshotDelay); 
                    break;
                default:
                    action(c);
                    break;
            }
        }

        private void OnFiller(Color c, Color target)
        {
            for (var i = 0; i < 4; i++)
            {
                var v = i switch
                {
                    0 => Vector2.up,
                    1 => Vector2.down,
                    2 => Vector2.left,
                    3 => Vector2.right,
                    _ => Vector2.zero
                };
                var hit = Physics2D.Raycast(transform.position, v);
                _image.color = c;
                if (hit.collider == null) continue;
                var click = hit.transform.gameObject.GetComponent<ClickOnPixel>();
                if (click._image.color != target) continue;
                click._image.color = c;
                click.OnFiller(c, target); 
            }
        }
    }
}
