using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Playing.Hints;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using _Scripts.SharedOverall.UI;
using _Scripts.SharedOverall.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Animating
{
    public class ClipManager : ClipPlaying
    {
        [SerializeField] private Button submitButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private ResultCorrector resultCorrector;
        
        public static event Action<WarningUI.WarningType> ShowWarning;
        public static event Action<Vector3, bool> SetHelpDirection;
        public static event Action<bool> SwitchPause;
        public static event Action SkipHint;

        private const int LevelStartDelay = 3;
        public static bool IsReadyToStart;
        public static bool IsHintPlayed;
        
        public enum State
        {
            Pause, Start
        }

        private void Start()
        {
            IsHintPlayed = false;
            IsReadyToStart = false;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            ClickOnPixel.PauseOrStartClip += PauseOrStartClip;
            ResultCorrector.SwitchSkipButton += SwitchSkipButton;
            ClipHint.RepeatClip += PlayClip;
            SelectableTool.StartLevel += StartLevel;
        }
        private new void OnDisable()
        {
            base.OnDisable();
            ClickOnPixel.PauseOrStartClip -= PauseOrStartClip;
            ResultCorrector.SwitchSkipButton -= SwitchSkipButton;
            ClipHint.RepeatClip -= PlayClip;
            SelectableTool.StartLevel -= StartLevel;
        }

        public async void SetClip()
        {
            await Task.WhenAll(ChangeClip(LevelCreator.GetCurrentStageScOb().animationClip));
            PlayClip();
        }

        private void PlayClip()
        {
            StartCoroutine(ClipCoroutine());
        }

        private IEnumerator ClipCoroutine()
        {
            animator.Play(0, 0, 0);
            if (LevelCreator.Stage == 0)
            {
                TextHintInvoker.Invoke(TextHintInvoker.HintType.Learn,0);
                yield return new WaitUntil(() => !TextHint.IsAnimating);
                IsHintPlayed = true;
            }
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.enabled = true;
            SwitchSkipButton(true);
        }

        private State PauseOrStartClip()
        {
            switch (IsReadyToStart)
            {
                case true:
                    StartLevel();
                    return State.Start;
                default:
                    var state = animator.enabled;
                    SetAnimatorState(!state);
                    SwitchPause?.Invoke(state);
                    return State.Pause;
            }
        }

        public void SkipClipClick()
        {
            if (PlayerPrefs.GetInt("ClipWarning",0) == 1)
            {
                SkipClip();
            }
            else
            {
                ShowWarning?.Invoke(WarningUI.WarningType.Skip);
            }
        }

        public void SkipClip()
        {
            SwitchSkipButton(false);
            SkipHint?.Invoke();
            switch (GameStateManager.CurrentGameState)
            {
                case GameStateManager.GameState.Animating:
                    if (animator.enabled == false)
                    {
                        SetAnimatorState(true);
                        SwitchPause?.Invoke(false);
                    }
                    animator.Play(0, 0, 1);
                    break;
                case GameStateManager.GameState.Correcting:
                    resultCorrector.SkipCorrection();
                    break;
            }
        }
        private void StartLevel()
        {
            if(!IsReadyToStart) return;
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            if (LevelCreator.Stage == 0)
            {
                TextHintInvoker.Invoke(TextHintInvoker.HintType.Do,3);
            }
            ProgressController.ToggleSliderState(false);
            StopCoroutine(clipTimer);
            SetHelpDirection?.Invoke(Vector3.zero, false);
            
            LevelCreator.IsGameStarted = true;
            animator.enabled = false;
            submitButton.interactable = true;
            SwitchSkipButton(false);
            
            if (LevelCreator.Stage == 0)
            {
                ToolsManager.CurrentTool = ToolsManager.Tools.None;
                ToolsManager.DeselectColors();
                ToolsManager.DeselectTools();
            }
            else
            {
                if (!(ToolsManager.GetTool(ToolsManager.Tools.Pencil).IsSelected() && ToolsManager.CurrentTool == ToolsManager.Tools.Pencil))
                {
                    ToolsManager.CurrentTool = ToolsManager.Tools.None;
                    ToolsManager.DeselectTools();
                }

                if (ColorPresetSpawner.ColorPresets.Exists(c => c.GetImageColor() == ResultCorrector.SelectedColorCash))
                {
                    var cp = ColorPresetSpawner.GetByColor(ResultCorrector.SelectedColorCash.Value);
                    cp.SelectWithoutAnimation();
                    ColorPreset.SetColor(cp.GetImageColor());
                }
                else
                {
                    ToolsManager.DeselectColors();
                }
            }
            switch (ClipHint.IsHint)
            {
                case true:
                    LoadState();
                    ClipHint.IsHint = false;
                    break;
                default:
                    LoadPreviousState();
                    break;
            }
            IsReadyToStart = false;
        }
        
        
        private void SwitchSkipButton(bool state)
        {
            skipButton.gameObject.SetActive(state);
        }

        public void SetAnimatorState(bool state)
        {
            if (animator.enabled == state) return;
            animator.enabled = state;
        }

        protected override void StartTimer()
        {
            if(!IsHintPlayed) return;
            IsReadyToStart = true;
            skipButton.gameObject.SetActive(false);
            SetHelpDirection?.Invoke(Vector3.zero, false);
            base.StartTimer();
        }

        protected override IEnumerator ClipTimer()
        {
            for (float i = 0; i < LevelStartDelay; i+=Time.deltaTime)
            {
                if (BlurManager.IsBlured())
                {
                    yield return new WaitUntil(() => !BlurManager.IsBlured());
                }
                yield return null;
            }
            ProgressController.ToggleSliderState(false);
            StartLevel();
        }
    }
}
