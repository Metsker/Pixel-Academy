using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.GeneralLogic.Menu.Logic
{
    public class AchievementsLoader : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] private Image[] worm;
        [SerializeField] private Image[] kitty;
        [SerializeField] private Image[] shark;
        [SerializeField] private Image[] inquisitive;
        [SerializeField] private Image[] lifeEnjoyer;
        [SerializeField] private Image[] luckyOne;
        [SerializeField] private Image[] rocket;
        [SerializeField] private Image[] happyGuy;
        [Header("Sprites")]
        [SerializeField] private Sprite activeWorm;
        [SerializeField] private Sprite activeKitty;
        [SerializeField] private Sprite activeShark;
        [SerializeField] private Sprite activeInquisitive;
        [SerializeField] private Sprite activeLifeEnjoyer;
        [SerializeField] private Sprite activeLuckyOne;
        [SerializeField] private Sprite activeRocket;
        [SerializeField] private Sprite activeHappyGuy;
        [SerializeField] private Sprite activeShape;
        [Header("TextFields")]
        [SerializeField] private TextMeshProUGUI winStreak;
        [SerializeField] private TextMeshProUGUI levelsCompleted;
        [SerializeField] private TextMeshProUGUI starsEarned;

        private const int AchievementsCount = 8;
        private const int ProgressCount = 3;
        
        private void Start()
        {
            for (var i = 0; i < ProgressCount; i++)
            {
                switch (i)
                {
                    case 0:
                        SetProgress("WinStreak", winStreak);
                        break;
                    case 1:
                        SetProgress("LevelsCompleted", levelsCompleted);
                        break;
                    case 2:
                        SetProgress("StarsEarned", starsEarned);
                        break;
                }
            }
            for (var i = 0; i < AchievementsCount; i++)
            {
                switch (i)
                {
                    case 0:
                        SetAchievement("Worm", worm, activeWorm);
                        break;
                    case 1:
                        SetAchievement("Kitty", kitty, activeKitty);
                        break;
                    case 2:
                        SetAchievement("Shark", shark, activeShark);
                        break;
                    case 3:
                        SetAchievement("Inquisitive", inquisitive, activeInquisitive);
                        break;
                    case 4:
                        SetAchievement("LifeEnjoyer", lifeEnjoyer, activeLifeEnjoyer);
                        break;
                    case 5:
                        SetAchievement("LuckyOne", luckyOne, activeLuckyOne);
                        break;
                    case 6:
                        SetAchievement("Rocket", rocket, activeRocket);
                        break;
                    case 7:
                        SetAchievement("HighAchiever", happyGuy, activeHappyGuy);
                        break;
                }
            }
        }

        private void SetAchievement(string key, Image[] array, Sprite sprite)
        {
            if (PlayerPrefs.GetInt(key, 0) != 1) return;
            for (var i = 0; i < array.Length; i++)
            {
                array[i].sprite = i == 0 ? activeShape : sprite;
            }
        }

        private void SetProgress(string key, TextMeshProUGUI text)
        {
            text.SetText($"« {PlayerPrefs.GetInt(key,0)} »");
        }
    }
}