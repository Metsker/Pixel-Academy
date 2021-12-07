using System;
using _Scripts.GameplayMod.Creating;
using _Scripts.GeneralLogic.Animating;
using _Scripts.GeneralLogic.Audio;
using _Scripts.GeneralLogic.ColorPresets;
using _Scripts.GeneralLogic.Tools.Instruments;
using _Scripts.GeneralLogic.Tools.Palette;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if (UNITY_EDITOR)
using _Scripts.EditorMod.Recording;
#endif

namespace _Scripts.GeneralLogic.Tools.Logic
{
    public class ClickOnPixel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject grid;
        
        private BoxCollider2D _boxCollider2D;
        private Color _cashColor;
        private RectTransform _cash;
        
        private static BoxCollider2D _viewCollider;
        public int index { get; set; }
        public Image image { get; set; }
        
        public static event Action StopClip;
        public static event Action<Vector3, bool> SetHelpDirection;
        private void Awake()
        {
            image = GetComponent<Image>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            
            if (_viewCollider != null) return;
            var parent = transform.parent;
            _viewCollider = parent.GetComponentInParent<BoxCollider2D>();
            _viewCollider.size = parent.GetComponentInParent<RectTransform>().rect.size;
            
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Play when
                    GameStateManager.CurrentGameState == GameStateManager.GameState.Animating:
                    StopClip?.Invoke();
                    break;
                default:
                    if (PickerHandler.IsPickerActive())
                    {
                        PickerHandler.DisablePicker();
                    }
                    break;
            }
            if (ToolsManager.CurrentTool == ToolsManager.Tools.None) return;
            switch (ToolsManager.CurrentTool)
            {
                case ToolsManager.Tools.Pencil:
                    OnPointer(PencilTool.GetColor(), c=> image.color = c);
                    break;
                case ToolsManager.Tools.Eraser when GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play && LevelCreator.Stage > 0:
                    OnPointer(EraserTool.GetColor(index),c=> image.color = c);
                    break;
                case ToolsManager.Tools.Eraser:
                    OnPointer(EraserTool.GetColor(),c=> image.color = c);
                    break;
                case ToolsManager.Tools.Filler:
                    var targetColor = image.color;
                    OnPointer(FillingTool.GetColor(), c => OnFiller(c, targetColor));
                    break;
                case ToolsManager.Tools.Pipette:
                    PencilTool.SetColor(image.color);
                    ColorPresetSpawner.GetSelected().image.color = image.color;
                    ToolsManager.CurrentTool = ToolsManager.Tools.Pencil;
                    break;
            }
        }

        private void Update()
        {
            if (!LevelCreator.isGameStarted)
            {
                _cashColor = image.color;
            }
            if (_cash == null || !_viewCollider.bounds.Contains(_cash.position)) return;
            SetHelpDirection?.Invoke(Vector3.zero, false);
            _cash = null;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            switch (LevelCreator.isGameStarted)
            {
                case false when _cashColor != image.color && !_viewCollider.bounds.Contains(position):
                {
                    SetHelpDirection?.Invoke(position, true);
                    _cash = (RectTransform)transform;
                    break;
                }
                case false when _cashColor != image.color && _viewCollider.bounds.Contains(position):
                    _cash = (RectTransform)transform;
                    break;
            }
        }

        private void OnPointer(Color c, Action<Color> action) 
        {
            if (image.color == c) return;
            switch (GameStateManager.CurrentGameState)
            {
                case GameStateManager.GameState.Recording when ToolAnimation.isAnyAnimating:
                    Debug.LogWarning("Дождись окончания анимации");
                    return;
#if (UNITY_EDITOR) 
                case GameStateManager.GameState.Recording:
                    action(c);
                    Recorder.Snapshot(AudioClick.AudioClickType.Click);
                    Recorder.Snapshot(Time.deltaTime); 
                    Recorder.Snapshot(Recorder.SnapshotDelay); 
                    break;
#endif
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
                image.color = c;
                if (hit.collider == null) continue;
                if (!hit.transform.gameObject.TryGetComponent(out ClickOnPixel click)) continue;
                if (click.image.color != target) continue;
                click.image.color = c;
                click.OnFiller(c, target);
            }
        }
        public void ToggleGrid(bool state)
        {
            grid.SetActive(state);
        }
        public void SetColliderSize()
        {
            _boxCollider2D.size = ((RectTransform)transform).sizeDelta;
        }
    }
}
