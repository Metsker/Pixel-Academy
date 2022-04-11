using System;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Playing.Animating
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public static event Action TimerEvent;
        public static event Action CheckProgress;
        public static event Action<AudioManager.AudioClickType> PlaySound;

        private void OnEnable()
        {
            ClipPlaying.LoadEvents += LoadEvents;
        }
        private void OnDisable()
        {
            ClipPlaying.LoadEvents -= LoadEvents;
        }
        private void LoadEvents(AnimationClip clip)
        {
            clip.AddEvent(new AnimationEvent {time = 0.1f, functionName = "StartEvent"});
            clip.AddEvent(new AnimationEvent {time = clip.length, functionName = "EndEvent"});
            
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) return;
            foreach (var t in LevelCreator.GetCurrentStageScOb().audioClickTimings)
            {
                clip.AddEvent(new AnimationEvent {time = t, functionName = "ClickEvent"});
            }
            foreach (var t in LevelCreator.GetCurrentStageScOb().audioToolTimings)
            {
                clip.AddEvent(new AnimationEvent {time = t, functionName = "ToolEvent"});
            }
        }

        private void StartEvent()
        {
            CheckProgress?.Invoke();
        }
        
        private void EndEvent()
        {
            TimerEvent?.Invoke(); 
        }
        
        private void ClickEvent()
        {
            PlaySound?.Invoke(AudioManager.AudioClickType.Click);
        }
        
        private void ToolEvent()
        {
            PlaySound?.Invoke(AudioManager.AudioClickType.Tool);
        }
    }
}
