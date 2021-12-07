using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Playing.Resulting.UI
{
    public class LevelCompleter : MonoBehaviour
    {
        [Header("Drawing result")]
        [SerializeField] private GameObject completeUI;
        [SerializeField] private FlexibleGridLayout resultGrid;
        [SerializeField] private FlexibleGridLayout drawingGrid;
        [SerializeField] private ScrollUI scrollUI;
        
        [SerializeField] private GameObject pxPrefab;
        [SerializeField] private GameObject resultView;
        
        public static event Action SwitchBlur;
        public static event Action<AudioEffects.AudioEffectType> PlaySound;
        public static List<Image> ResultPixels { get; private set; }

        private void OnEnable()
        {
            RewardCalculator.CompleteLevel += Complete;
        }
        private void OnDisable()
        {
            RewardCalculator.CompleteLevel -= Complete;
        }

        private void Complete(RewardCalculator.Result result, bool needRef)
        {
            LevelCreator.isGameStarted = false;
            scrollUI.enabled = false;
            if (AudioSettings.IsMusicEnabled())
            {
                switch (result)
                {
                    case RewardCalculator.Result.NotPassed:
                        PlaySound?.Invoke(AudioEffects.AudioEffectType.LevelNotPassed);
                        break;
                    case RewardCalculator.Result.Passed:
                        PlaySound?.Invoke(AudioEffects.AudioEffectType.LevelPassed);
                        break;
                    case RewardCalculator.Result.Perfect:
                        PlaySound?.Invoke(AudioEffects.AudioEffectType.LevelPerfect);
                        break;
                }
            }
            ToggleEndScreen(true);
            BuildPixels();
            if(!needRef) return;
            resultView.SetActive(true);
        }
        public void ToggleEndScreen(bool state)
        {
            SwitchBlur?.Invoke();
            completeUI.SetActive(state);
        }
        private async void BuildPixels()
        {
            ResultPixels = new List<Image>();
            resultGrid.columns = drawingGrid.columns;
            for (var i = 0; i < DrawingTemplateCreator.ImagesList.Count; i++)
            {
                var obj = Instantiate(pxPrefab, resultGrid.transform);
                obj.name = $"Px ({i}) Result";
                var image = obj.GetComponent<Image>();
                ResultPixels.Add(image);
                if (ClipPlaying.CashImageStates == null) continue;
                image.color = ClipPlaying.CashImageStates[i];
            }
            await resultGrid.SetSize(false);
        }
    }
}