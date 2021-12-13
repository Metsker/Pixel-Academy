using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Instruments;
using _Scripts.SharedOverall.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Animating
{
    public abstract class ClipPlaying : MonoBehaviour
    {
        public Animator animator;
        [SerializeField] protected Slider progress;
        private Image _progressImage;
        public static List<Color> CashImageStates { get; private set; }
        private static List<Color> CashColorPresetStates { get; set; }
        public static event Action<AnimationClip> LoadEvents;

        protected const int StartDelaySeconds = 4;
        protected IEnumerator clipTimer;

        protected void Awake()
        {
            _progressImage = progress.fillRect.GetComponent<Image>();
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
            _progressImage.color = ColorRandomizer.GetRandomColor();
            while (GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                progress.value = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
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
            foreach (var preset in ColorPresetSpawner.colorPresets)
            {
                CashColorPresetStates.Add(preset.image.color);
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
            foreach (var preset in ColorPresetSpawner.colorPresets)
            {
                preset.image.color = CashColorPresetStates[i];
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
        protected abstract IEnumerator ClipTimer();
    }
}
