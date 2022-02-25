using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Instruments;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Saving;
using _Scripts.SharedOverall.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Animating
{
    public abstract class ClipPlaying : MonoBehaviour
    {
        public Animator animator;
        private static List<Color> CashImageStates { get; set; }
        private static List<Color> CashColorPresetStates { get; set; }
        public static event Action<AnimationClip> LoadEvents;
        
        protected IEnumerator clipTimer;

        protected void Awake()
        {
            if (SaveData.ClipSliderValue == null) return;
            animator.speed = Mathf.Lerp(ClipSpeedSlider.ClipMin,ClipSpeedSlider.ClipMax, (float)SaveData.ClipSliderValue);
        }

        protected void OnEnable()
        {
            AnimationEventHandler.CheckProgress += CheckAnimationProgress;
            AnimationEventHandler.TimerEvent += StartTimer;
        }

        protected void OnDisable()
        {
            AnimationEventHandler.CheckProgress -= CheckAnimationProgress;
            AnimationEventHandler.TimerEvent -= StartTimer;
        }
        
        protected Task ChangeClip(AnimationClip clip)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = aoc.animationClips.Select(a => 
                new KeyValuePair<AnimationClip, AnimationClip>(a, clip)).ToList();
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
            ClearEvents(clip);
            return Task.CompletedTask;
        }
        private void ClearEvents(AnimationClip clip)
        {
            var myEvents = clip.events;
            if (myEvents.Length > 0)
            {
                var list = myEvents.ToList();
                list.Clear();
                clip.events = list.ToArray();
            }
            LoadEvents?.Invoke(clip);
        }

        private void CheckAnimationProgress()
        {
            StartCoroutine(AnimationProgress());
        }
        
        private IEnumerator AnimationProgress()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record)
            {
                ProgressController.SetProgressColor(ProgressController.AnimationColor);
                ProgressController.ToggleSliderState(true);
            }
            while (GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                GetSlider().normalizedValue = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                yield return null;
            }
        }
        public static void SaveState()
        {
            CashImageStates = new List<Color>();
            foreach (var img in DrawingTemplateCreator.ImagesList)
            {
                CashImageStates.Add(img.color);
            }
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record) return;
            CashColorPresetStates = new List<Color>();
            foreach (var preset in ColorPresetSpawner.ColorPresets)
            {
                CashColorPresetStates.Add(preset.GetImageColor());
            }
        }

        protected void LoadState()
        {
            var i = 0;
            foreach (var img in DrawingTemplateCreator.ImagesList)
            {
                img.color = CashImageStates[i];
                i++;
            }
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record) return;
            i = 0;
            foreach (var preset in ColorPresetSpawner.ColorPresets)
            {
                preset.SetImageColor(CashColorPresetStates[i]);
                i++;
            }
        }
        
        protected void LoadPreviousState()
        {
            if (LevelCreator.Stage - 1 < 0)
            {
                ClearTool.Clear();
                return;
            }
            var i = 0;
            foreach (var img in DrawingTemplateCreator.ImagesList)
            {
                img.color = LevelCreator.GetPreviousStageScOb().pixelList[i];
                i++;
            }
        }
        
        protected virtual void StartTimer()
        {
            clipTimer = ClipTimer();
            StartCoroutine(clipTimer);
        }

        protected virtual Slider GetSlider()
        {
            return ProgressController.Slider;
        }
        protected abstract IEnumerator ClipTimer();

        
    }
}
