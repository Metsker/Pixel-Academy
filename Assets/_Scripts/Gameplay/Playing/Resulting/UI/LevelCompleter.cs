using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Resulting.UI
{
    public class LevelCompleter : MonoBehaviour
    {
        [Header("Drawing result")]
        [SerializeField] private GameObject completeUI;
        [SerializeField] private FlexibleGridLayout resultGrid;
        [SerializeField] private FlexibleGridLayout drawingGrid;

        [SerializeField] private GameObject pxPrefab;
        [SerializeField] private GameObject resultView;
        
        public static bool IsLevelCompleted { get; private set; }
        public static event Action<bool> SwitchBlur;
        public static event Action<AudioEffects.AudioEffectType> PlaySound;
        public static event Action ShowAd;
        public static List<Image> ResultImages { get; private set; }
        public static List<Color> ResultColors { get; private set; }

        private void Start()
        {
            IsLevelCompleted = false;
        }

        private void OnEnable()
        {
            RewardCalculator.CompleteLevel += Complete;
        }
        private void OnDisable()
        {
            RewardCalculator.CompleteLevel -= Complete;
        }

        private void Complete(RewardCalculator.Result result)
        {
            LevelCreator.IsGameStarted = false;
            IsLevelCompleted = true;
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
            ShowAd?.Invoke();
            
            if (MusicToggler.IsMusicEnabled())
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
            PlayerPrefs.SetInt("CanGain", 1);
            PlayerPrefs.Save();
            if(result == RewardCalculator.Result.Perfect) return;
            resultView.SetActive(true);
        }
        public void ToggleEndScreen(bool state)
        {
            SwitchBlur?.Invoke(true);
            completeUI.SetActive(state);
        }
        private async void BuildPixels()
        {
            ResultImages = new List<Image>();
            ResultColors = new List<Color>();
            
            resultGrid.columns = drawingGrid.columns;
            for (var i = 0; i < DrawingTemplateCreator.ImagesList.Count; i++)
            {
                var obj = Instantiate(pxPrefab, resultGrid.transform);
                obj.name = $"Px ({i}) Result";
                var image = obj.GetComponent<Image>();
                image.color = DrawingTemplateCreator.PixelList[i].IsWrong
                    ? (Color) ResultCorrector.MistakeColor
                    : LevelCreator.GetCurrentStageScOb().pixelList[i];
                ResultImages.Add(image);
                ResultColors.Add(image.color);
            }
            await resultGrid.SetSize(false);
        }
    }
}