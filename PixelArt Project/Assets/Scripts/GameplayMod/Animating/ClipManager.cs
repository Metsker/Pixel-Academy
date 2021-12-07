using System;
using System.Collections;
using System.Threading.Tasks;
using GameplayMod.Creating;
using GameplayMod.Resulting;
using GeneralLogic;
using GeneralLogic.Animating;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayMod
{
    public class GameStateToggler : ClipPlaying
    {
        [SerializeField] private Button submitButton;
        [SerializeField] private Button skipButton;
        public static bool isGameStarted { get; set; }
        private const int DelaySeconds = 4;
        private bool _isPaused;
        private IEnumerator _clipTimer;

        private void OnEnable()
        {
            ClickOnPixel.StopClip += StopClip;
            ResultComparer.SwitchSkipButton += SwitchSkip;
            AnimationEventHandler.TimerEvent += StartTimer;
        }

        private void OnDisable()
        {
            ClickOnPixel.StopClip -= StopClip;
            ResultComparer.SwitchSkipButton -= SwitchSkip;
            AnimationEventHandler.TimerEvent -= StartTimer;
        }

        public void Start()
        {
            isGameStarted = false;
        }

        public async void SetClip()
        {
            ChangeClip(LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].animationClip);
            await Task.Yield();
            PlayClip();
            await Task.Yield();
            SaveState();
            isGameStarted = false;
        }

        private void PlayClip()
        {
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.Rebind();
            animator.enabled = true;
            progress.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            submitButton.interactable = false;
        }

        private void StopClip()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
                && !animator.IsInTransition(0))
            {
                StartLevel();
            }
            else
            {
                SwitchPause();
            }
        }

        private void SwitchPause()
        {
            animator.enabled = !animator.enabled; //значек паузы
            _isPaused = !_isPaused; //новая кнопка скипа
        }
        public void SkipClip()
        {
            if (_isPaused)
            {
                SwitchPause();
            }
            switch (animator.enabled)
            {
                case true:
                    animator.Play(0, 0, 1);
                    break;
                case false:
                    skipButton.gameObject.SetActive(false);
                    ResultComparer.Skip = true;
                    StartCoroutine(CorrectionTimer());
                    break;
            }
        }

        private void StartTimer()
        {
            skipButton.gameObject.SetActive(false);
            _clipTimer = ClipTimer();
            StartCoroutine(_clipTimer);
        }
        
        private void SwitchSkip()
        {
            skipButton.gameObject.SetActive(!skipButton.gameObject.activeSelf);
        }
        private void StartLevel()
        {
            if (_clipTimer != null)
            {
                StopCoroutine(_clipTimer);
            }
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            animator.enabled = false;
            isGameStarted = true;
            submitButton.interactable = true;
            progress.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            ToolsManager.ResetActiveTool();
            LoadState();
        }
        private IEnumerator ClipTimer()
        {
            yield return new WaitForSeconds(DelaySeconds);
            StartLevel();
        }

        private IEnumerator CorrectionTimer()
        {
            yield return new WaitForSeconds(0.5f);
            ResultComparer.Skip = false;
        }
    }
}
