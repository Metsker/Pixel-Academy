using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.Saving;
using _Scripts.SharedOverall.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Playing.Resulting
{
    public abstract class RewardCalculator : AchievementsHandler
    {
        [SerializeField] private List<Image> stars;
        [SerializeField] private Sprite filledStar;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Color perfectColor;
        
        private Result _result;
        
        public static event Action<Result, bool> CompleteLevel;
        public static event Action<AudioEffects.AudioEffectType> PlaySound;

        private readonly Dictionary<string, string> _ruDictionary = new()
        {
            { "Perfect", "Идеально!" },
            { "Great", "Отлично!" },
            { "Good", "Хорошо." },
            { "OK", "Не плохо." },
            { "Bad", "Плохо." },
            { "NotPassed", "Слишком много ошибок. :(" },
            
            { "TokensEarned", "\nТокенов заработано:" }
        };
        private readonly Dictionary<string, string> _engDictionary = new()
        {
            { "Perfect", "Perfect!" },
            { "Great", "Great!" },
            { "Good", "Good." },
            { "OK", "OK." },
            { "Bad", "Bad." },
            { "NotPassed", "Too many mistakes. :(" },
            
            { "TokensEarned", "\nTokens earned:" }
        };
        public enum Result
        {
            NotPassed,
            Passed,
            Perfect
        }
        protected async void CalculateStars(int mistakesCount, bool lastStageMistake)
        {
            var pxCount = LevelCreator.GetCurrentStageScOb().pixelList.Count(img => img != Color.white);
            var result = (float)(pxCount - mistakesCount) / pxCount;
            int mark;
            
            switch (mistakesCount)
            {
                case 0 :
                    mark = 5;
                    resultText.color = perfectColor;
                    SetResult(GetLocalizedString("Perfect"), Result.Perfect, false);
                    break;
                case var _ when result > 0.875f:
                    mark = 4;
                    SetResult(GetLocalizedString("Great"), Result.Passed, lastStageMistake);
                    break;
                case var _ when result <= 0.875f && result > 0.75f:
                    mark = 3;
                    SetResult(GetLocalizedString("Good"), Result.Passed, lastStageMistake);
                    break;
                case var _ when result <= 0.75f && result > 0.625f:
                    mark = 2;
                    SetResult(GetLocalizedString("OK"), Result.Passed, lastStageMistake);
                    break;
                case var _ when result <= 0.625f && result > 0.5f:
                    mark = 1;
                    SetResult(GetLocalizedString("Bad"), Result.Passed, lastStageMistake);
                    break;
                default:
                    PlayerPrefs.SetInt("WinStreak", 0);
                    SetResult(GetLocalizedString("NotPassed"), Result.NotPassed, lastStageMistake);
                    return;
            }
            
            CalculateReward(mark);

            if (MusicToggler.IsMusicEnabled())
            {
                await Task.Delay((int)(AudioEffects.GetPassedSoundLength(_result)*1000));
            }

            for (var i = 0; i < mark; i++)
            {
                await Task.Delay(150);
                PlaySound?.Invoke(AudioEffects.AudioEffectType.Stars);
                stars[i].sprite = filledStar;
            }
            SaveData.SaveLevelData(LevelCreator.scriptableObject);
            SaveSystem.SaveDataToFile();
        }

        private void CalculateReward(int mark)
        {
            if (LevelCreator.scriptableObject.stars < mark)
            {
                SetAchievementsProgress(mark - LevelCreator.scriptableObject.stars);
                LevelCreator.scriptableObject.stars = mark;
                if (mark != 3) return;
                PlayerPrefs.SetInt("PerfectCompleted", PlayerPrefs.GetInt("PerfectCompleted",0)+1);
                PlayerPrefs.Save();
                switch (LevelCreator.scriptableObject.difficulty)
                {
                    case DifficultyFilterManager.Difficulties.Easy:
                        AddTokens(1);
                        break;
                    case DifficultyFilterManager.Difficulties.Medium:
                        AddTokens(2);
                        break;
                    case DifficultyFilterManager.Difficulties.Hard:
                        AddTokens(3);
                        break;
                }
            }
            else
            {
                SetAchievementsProgress(0);
            }
        }
        
        private void SetResult(string resultTxt, Result result, bool lastStageMistake)
        {
            resultText.text = resultTxt;
            _result = result;
            CompleteLevel?.Invoke(_result, lastStageMistake);
        }
        
        private void AddTokens(int value)
        {
            PlayerPrefs.SetInt("HintTokens", PlayerPrefs.GetInt("HintTokens", 3) + value);
            PlayerPrefs.Save();
            resultText.text += $"{GetLocalizedString("TokensEarned")} {value}";
        }
        private string GetLocalizedString(string index)
        {
            return Application.systemLanguage == SystemLanguage.Russian ? _ruDictionary[index] : _engDictionary[index];
        }
    }
}