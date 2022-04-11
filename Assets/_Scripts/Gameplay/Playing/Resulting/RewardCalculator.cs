using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.Saving;
using _Scripts.SharedOverall.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static _Scripts.SharedOverall.Dictionaries;

namespace _Scripts.Gameplay.Playing.Resulting
{
    public abstract class RewardCalculator : AchievementsHandler
    {
        [SerializeField] private List<Image> stars;
        [SerializeField] private Sprite filledStar;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Color perfectColor;
        
        private Result _result;

        public static event Action RateRequest;
        public static event Action<Result> CompleteLevel;
        public static event Action<AudioEffects.AudioEffectType> PlaySound;
        
        public enum Result
        {
            NotPassed,
            Passed,
            Perfect
        }
        protected async void CalculateStars(int mistakesCount)
        {
            var pxCount = LevelCreator.GetCurrentStageScOb().pixelList.Count(img => img != Color.white);
            var result = (float)(pxCount - mistakesCount) / pxCount;
            int mark;
            
            switch (mistakesCount)
            {
                case 0:
                    mark = 5;
                    SetResult(GetLocalizedString("Perfect"), Result.Perfect);
                    break;
                case var _ when result > 0.875f:
                    mark = 4;
                    SetResult(GetLocalizedString("Great"), Result.Passed);
                    break;
                case var _ when result <= 0.875f && result > 0.75f:
                    mark = 3;
                    SetResult(GetLocalizedString("Good"), Result.Passed);
                    break;
                case var _ when result <= 0.75f && result > 0.625f:
                    mark = 2;
                    SetResult(GetLocalizedString("OK"), Result.Passed);
                    break;
                case var _ when result <= 0.625f && result > 0.5f:
                    mark = 1;
                    SetResult(GetLocalizedString("Bad"), Result.Passed);
                    break;
                default:
                    PlayerPrefs.SetInt("WinStreak", 0);
                    SetResult(GetLocalizedString("NotPassed"), Result.NotPassed);
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
            SaveData.SaveLevelData(LevelCreator.ScriptableObject);
            SaveSystem.SaveDataToFile();
            RateRequest?.Invoke();
        }

        private void CalculateReward(int mark)
        {
            var coinsCount = Mathf.RoundToInt(mark * ((int) LevelCreator.ScriptableObject.difficulty + 1));
            if (LevelCreator.ScriptableObject.stars < mark)
            {
                SetAchievementsProgress(mark - LevelCreator.ScriptableObject.stars);
                AddCoins(coinsCount);
                LevelCreator.ScriptableObject.stars = mark;
                
            }
            else
            {
                SetAchievementsProgress(0);
                AddCoins(coinsCount/2);
            }
        }
        
        private void SetResult(string resultTxt, Result result)
        {
            resultText.text = resultTxt;
            _result = result;
            CompleteLevel?.Invoke(_result);
        }
        
        private void AddCoins(int value)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + value);
            PlayerPrefs.Save();
            resultText.text += $"{GetLocalizedString("CoinsEarned")} {value}";
        }
    }
}