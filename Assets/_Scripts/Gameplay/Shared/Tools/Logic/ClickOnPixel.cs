using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.SharedOverall.GameModeManager;
using static _Scripts.SharedOverall.GameStateManager;
#if (UNITY_EDITOR)
using _Scripts.Gameplay.Recording.Recording;
#endif

namespace _Scripts.SharedOverall.Tools.Logic
{
    public class ClickOnPixel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject grid;
        
        private BoxCollider2D _boxCollider2D;
        private Color _cashColor;
        private RectTransform _cash;
        public int Index { get; set; }
        public bool IsWrong { get; set; }
        private Image Image { get; set; }
        private static BoxCollider2D _viewCollider;
        public static event Func<ClipManager.State> PauseOrStartClip;
        public static event Action SetPipetteColor;
        public static event Action<Vector3, bool> SetHelpDirection;

        private void Awake()
        {
            Image = GetComponent<Image>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            
            if (_viewCollider != null) return;
            var parent = transform.parent;
            _viewCollider = parent.GetComponentInParent<BoxCollider2D>();
            _viewCollider.size = parent.GetComponentInParent<RectTransform>().rect.size;
        }
        
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public void OnPointerClick(PointerEventData eventData)
        {
            if (TextHint.IsAnimating)
            {
                TextHint.SkipTextAnim();
                return;
            }
            switch (CurrentGameMode)
            {
                case GameMode.Play when CurrentGameState == GameState.Animating:
                {
                    switch (PauseOrStartClip?.Invoke())
                    {
                        case ClipManager.State.Pause: 
                            return;
                        case ClipManager.State.Start:
                            break;
                    } break;
                }
                case GameMode.Play when
                    CurrentGameState == GameState.Correcting:
                    if (ResultCorrector.SetCorrectionState(!ResultCorrector.IsPaused)) { return; } 
                    else { break; }
                case GameMode.Play when !LevelCreator.IsGameStarted:
                    return;
                default:
                    if (!PickerHandler.IsPickerActive()) break;
                    PickerHandler.DisablePicker();
                    return;
            }
            if (ToolsManager.CurrentTool == ToolsManager.Tools.None && CurrentGameState == GameState.Drawing)
            {
                TextHintInvoker.Invoke
                    (TextHintInvoker.HintType.PickTool, 3);
                return;
            }
            switch (ToolsManager.CurrentTool)
            {
                case var _ when ToolsManager.CurrentTool != ToolsManager.Tools.Eraser && ColorPreset.GetColor() == null && CurrentGameState == GameState.Drawing:
                    TextHintInvoker.Invoke
                        (TextHintInvoker.HintType.PickColor,3);
                    break;
                case ToolsManager.Tools.Pencil when ColorPreset.GetColor() != null:
                    OnPointer(ColorPreset.GetColor().Value, c=> Image.color = c);
                    break;
                case ToolsManager.Tools.Eraser when CurrentGameMode == GameMode.Play && LevelCreator.Stage > 0:
                    OnPointer(EraserTool.GetColor(Index),c=> Image.color = c);
                    break;
                case ToolsManager.Tools.Eraser:
                    OnPointer(EraserTool.GetColor(),c=> Image.color = c);
                    break;
                case ToolsManager.Tools.Filler when ColorPreset.GetColor() != null:
                    var targetColor = Image.color;
                    OnPointer(ColorPreset.GetColor().Value, c => OnFiller(c, targetColor));
                    break;
                case ToolsManager.Tools.Pipette:
                    ColorPreset.SetColor(Image.color);
                    ColorPresetSpawner.GetSelected().SetImageColor(Image.color);
                    SetPipetteColor?.Invoke();
                    break;
            }
        }

        private void Update()
        {
            if (_cash == null || !_viewCollider.bounds.Contains(_cash.position)) return;
            SetHelpDirection?.Invoke(Vector3.zero, false);
            _cash = null;
        }

        private void FixedUpdate()
        {
            if (LevelCreator.IsGameStarted || _cashColor == Image.color) return;
            _cashColor = Image.color;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            switch (LevelCreator.IsGameStarted)
            {
                case false when _cashColor != Image.color && !_viewCollider.bounds.Contains(position):
                {
                    SetHelpDirection?.Invoke(position, true);
                    _cash = (RectTransform)transform;
                    break;
                }
                case false when _cashColor != Image.color && _viewCollider.bounds.Contains(position):
                    _cash = (RectTransform)transform;
                    break;
            }
        }

        private void OnPointer(Color c, Action<Color> action) 
        {
            if (Image.color == c) return;
            switch (CurrentGameState)
            {
                case GameState.Recording when ToolAnimation.isAnyAnimating:
                    Debug.LogWarning("Дождись окончания анимации");
                    return;
#if (UNITY_EDITOR) 
                case GameState.Recording:
                    action(c);
                    Recorder.Snapshot(AudioManager.AudioClickType.Click);
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
                Image.color = c;
                if (hit.collider == null) continue;
                if (!hit.transform.gameObject.TryGetComponent(out ClickOnPixel click)) continue;
                if (click.Image.color != target) continue;
                click.Image.color = c;
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
        public Image GetImage()
        {
            return Image;
        }
    }
}
