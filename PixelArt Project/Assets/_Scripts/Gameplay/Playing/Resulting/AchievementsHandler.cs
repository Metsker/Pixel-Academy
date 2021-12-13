using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Playing.Resulting
{
    public class AchievementsHandler : BaseTool
    {
        protected void SetAchievementsProgress(int mark)
        {
            if (!(LevelCreator.scriptableObject.stars > 0))
            {
                PlayerPrefs.SetInt("LevelsCompleted", PlayerPrefs.GetInt("LevelsCompleted")+1);
            }
            
            PlayerPrefs.SetInt("WinStreak", PlayerPrefs.GetInt("WinStreak")+1);
            
            for (var i = 0; i < mark; i++)
            {
                PlayerPrefs.SetInt("StarsEarned", PlayerPrefs.GetInt("StarsEarned")+1);
                PlayerPrefs.Save();
                switch (PlayerPrefs.GetInt("StarsEarned"))
                {
                    case 15:
                        if (PlayerPrefs.GetInt("Worm", 0) == 1) break;
                        PlayerPrefs.SetInt("Worm", 1);
                        PlayerPrefs.SetInt("HasNewAchievement", 1);
                        break;
                    case 50:
                        if (PlayerPrefs.GetInt("Kitty", 0) == 1) break;
                        PlayerPrefs.SetInt("Kitty", 1);
                        PlayerPrefs.SetInt("HasNewAchievement", 1);
                        break;
                    case 100:
                        if (PlayerPrefs.GetInt("Shark", 0) == 1) break;
                        PlayerPrefs.SetInt("Shark", 1);
                        PlayerPrefs.SetInt("HasNewAchievement", 1);
                        break;
                }
            }
            
            if (mark==0) PlayerPrefs.Save();

            switch (PlayerPrefs.GetInt("WinStreak"))
            {
                case 3:
                    if (PlayerPrefs.GetInt("Inquisitive", 0) == 1) break;
                    PlayerPrefs.SetInt("Inquisitive", 1);
                    PlayerPrefs.SetInt("HasNewAchievement", 1);
                    break;
                case 5:
                    if (PlayerPrefs.GetInt("LifeEnjoyer", 0) == 1) break;
                    PlayerPrefs.SetInt("LifeEnjoyer", 1);
                    PlayerPrefs.SetInt("HasNewAchievement", 1);
                    break;
                case 10:
                    if (PlayerPrefs.GetInt("LuckyOne", 0) == 1) break;
                    PlayerPrefs.SetInt("LuckyOne", 1);
                    PlayerPrefs.SetInt("HasNewAchievement", 1);
                    break;
            }
            
            switch (PlayerPrefs.GetInt("LevelsCompleted"))
            {
                case 5:
                    if (PlayerPrefs.GetInt("Rocket", 0) == 1) break;
                    PlayerPrefs.SetInt("Rocket", 1);
                    PlayerPrefs.SetInt("HasNewAchievement", 1);
                    break;
            }
            
            switch (PlayerPrefs.GetInt("PerfectCompleted"))
            {
                case 10:
                    if (PlayerPrefs.GetInt("HighAchiever", 0) == 1) break;
                    PlayerPrefs.SetInt("HighAchiever", 1);
                    PlayerPrefs.SetInt("HasNewAchievement", 1);
                    break;
            }
            PlayerPrefs.Save();
        }
    }
}