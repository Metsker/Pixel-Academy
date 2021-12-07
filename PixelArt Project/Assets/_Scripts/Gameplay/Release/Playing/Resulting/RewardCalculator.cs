using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.Data;
using _Scripts.SharedOverall.Saving;
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
            { "Passed", "Уровень пройден." },
            { "Perfect", "Идеально!" },
            { "NotPassed", "Слишком много ошибок. :(" },
            { "TokensEarned", "\nТокенов заработано:" }
        };
        private readonly Dictionary<string, string> _engDictionary = new()
        {
            { "Passed", "Level is passed." },
            { "Perfect", "Perfect!" },
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
                    mark = 3;
                    resultText.color = perfectColor;
                    SetResult(GetLocalizedString("Perfect"), Result.Perfect, false);
                    break;
                case var _ when result >= 0.82f: //?
                    mark = 2;
                    SetResult(GetLocalizedString("Passed"), Result.Passed, lastStageMistake);
                    break;
                case var _ when result >= 0.67f && result <= 0.81f: //?
                    mark = 1;
                    SetResult(GetLocalizedString("Passed"), Result.Passed, lastStageMistake);
                    break;
                default:
                    PlayerPrefs.SetInt("WinStreak", 0);
                    SetResult(GetLocalizedString("NotPassed"), Result.NotPassed, lastStageMistake);
                    return;
            }
            
            CalculateReward(mark);

            if (AudioSettings.IsMusicEnabled())
            {
                await Task.Delay((int)(AudioEffects.GetPassedSoundLength(_result)*1000));
            }

            for (var i = 0; i < mark; i++)
            {
                await Task.Delay(150);
                PlaySound?.Invoke(AudioEffects.AudioEffectType.Stars);
                stars[i].sprite = filledStar;
            }
            ScriptableObjectDataSaver.SaveData(LevelCreator.scriptableObject);
            SaveSystem.SaveData();
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
                AddTokens(2);

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