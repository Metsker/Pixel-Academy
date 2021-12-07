using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.GameplayMod.Creating;
using _Scripts.GameplayMod.Hints;
using _Scripts.GameplayMod.Resulting;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic;
using _Scripts.GeneralLogic.Animating;
using _Scripts.GeneralLogic.ColorPresets;
using _Scripts.GeneralLogic.Tools.Logic;
using _Scripts.GeneralLogic.Tools.Palette;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GameplayMod.Animating
{
    public class ClipManager : ClipPlaying
    {
        [SerializeField] private Button submitButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite unpauseSprite;
        
        public Image pauseImage;
        private const float AnimationDuration = 0.3f;
        private const float PauseOpacity = 0.7f;
        private static bool _isReadyToStart;
        private bool _isAnimating;

        public static event Action<WarningUI.WarningType> ShowWarning;
        public static event Action<Vector3, bool> SetHelpDirection;
        
        private void Start()
        {
            _isReadyToStart = false;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            ClickOnPixel.StopClip += StopClip;
            ResultCalculator.SwitchSkipButton += SwitchSkipButton;
            ClipHint.RepeatClip += PlayClip;
            ColorPreset.StartLevel += StartLevel;
        }
        private new void OnDisable()
        {
            base.OnDisable();
            ClickOnPixel.StopClip -= StopClip;
            ResultCalculator.SwitchSkipButton -= SwitchSkipButton;
            ClipHint.RepeatClip -= PlayClip;
            ColorPreset.StartLevel -= StartLevel;
        }

        public async void SetClip()
        {
            await Task.WhenAll(ChangeClip(LevelCreator.GetCurrentStageScOb().animationClip));
            PlayClip();
        }

        private void PlayClip()
        {
            LevelCreator.isGameStarted = false;
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.Play(0, 0, 0);
            animator.enabled = true;
            progress.value = 0;
            progress.gameObject.SetActive(true);
            SwitchSkipButton(true);
            submitButton.interactable = false;
        }

        private void StopClip()
        {
            switch (_isReadyToStart)
            {
                case true:
                    StartLevel();
                    break;
                default:
                    SwitchPauseWithAnimation(!animator.enabled);
                    break;
            }
        }

        public void SkipClipClick()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Drawing)
            {
                SkipClip();
            }
            else if (PlayerPrefs.GetInt("ClipWarning",0) == 1)
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
            switch (GameStateManager.CurrentGameState)
            {
                case GameStateManager.GameState.Animating:
                    SwitchPauseWithAnimation(true);
                    animator.Play(0, 0, 1);
                    break;
                case GameStateManager.GameState.Drawing:
                    ResultCalculator.Skip = true;
                    StartCoroutine(CorrectionTimer());
                    break;
            }
        }
        private void StartLevel()
        {
            if(!_isReadyToStart) return;
            StopCoroutine(clipTimer);
            SetHelpDirection?.Invoke(Vector3.zero, false);
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            LevelCreator.isGameStarted = true;
            animator.enabled = false;
            submitButton.interactable = true;
            SwitchSkipButton(false);
            progress.gameObject.SetActive(false);
            
            if (ResultCalculator.selectedColorCash == ToolsManager.ColorZero || !ColorPresetSpawner.colorPresets.Exists(c => c.image.color == ResultCalculator.selectedColorCash))
            {
                ToolsManager.CurrentTool = ToolsManager.Tools.None;
                ToolsManager.DeselectTools();
            }
            else
            {
                var cp = ColorPresetSpawner.GetByColor(ResultCalculator.selectedColorCash);
                ToolsManager.CurrentTool = ToolsManager.Tools.Pencil;
                cp.Select();
                ToolsManager.DeselectInstruments();
                PencilTool.SetColor(cp.image.color);
                ResultCalculator.selectedColorCash = ToolsManager.ColorZero;
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
            _isReadyToStart = false;
        }
        
        
        private void SwitchSkipButton(bool state)
        {
            skipButton.gameObject.SetActive(state);
        }

        private void SwitchPauseWithAnimation(bool animatorActiveState)
        {
            if(animator.enabled == animatorActiveState) return;
            animator.enabled = animatorActiveState;
            pauseImage.sprite = animatorActiveState ? pauseSprite : unpauseSprite;
            pauseImage.DOFade(PauseOpacity, AnimationDuration);
            DOTween.Sequence().AppendCallback(
                () =>
                {
                    if (_isAnimating) return;
                    _isAnimating = true;
                    pauseImage.gameObject.transform.DOPunchScale(Vector3.one * 0.25f, AnimationDuration, 1)
                        .OnComplete(() => _isAnimating = false);
                }).AppendInterval(0.6f).Append(pauseImage.DOFade(0, AnimationDuration));
        }
        
        public void SwitchPause(bool animatorActiveState)
        {
            if(animator.enabled == animatorActiveState) return;
            animator.enabled = animatorActiveState;
        }

        protected override void StartTimer()
        {
            _isReadyToStart = true;
            skipButton.gameObject.SetActive(false);
            SetHelpDirection?.Invoke(Vector3.zero, false);
            base.StartTimer();
        }

        protected override IEnumerator ClipTimer()
        {
            yield return new WaitForSeconds(StartDelaySeconds);
            StartLevel();
        }

        private IEnumerator CorrectionTimer()
        {
            yield return new WaitForSeconds(0.5f);
            ResultCalculator.Skip = false;
        }
    }
}
