using System;
using _Scripts.GameplayMod.Creating;
using _Scripts.GeneralLogic;
using _Scripts.GeneralLogic.Animating;
using _Scripts.GeneralLogic.Audio;
using UnityEngine;

namespace _Scripts.GameplayMod.Animating
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public static event Action TimerEvent;
        public static event Action CheckProgress;
        public static event Action<AudioClick.AudioClickType> PlaySound;

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
            PlaySound?.Invoke(AudioClick.AudioClickType.Click);
        }
        
        private void ToolEvent()
        {
            PlaySound?.Invoke(AudioClick.AudioClickType.Tool);
        }
    }
}
