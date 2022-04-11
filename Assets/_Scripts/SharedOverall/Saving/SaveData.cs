using System.Collections.Generic;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall.Settings;
using _Scripts.SharedOverall.UI.Settings;

namespace _Scripts.SharedOverall.Saving
{
    public static class SaveData
    {
        public static Dictionary<string, int> Stars = new();
        public static Dictionary<string, bool> Unlocks = new();
        public static float? ClipSliderValue;
        public static int? SelectedLocaleIndex;

        public static void SaveLevelData(LevelScriptableObject level)
        {
            if (Stars.ContainsKey(level.name))
            {
                Stars[level.name] = level.stars;
            }
            else
            {
                Stars.Add(level.name, level.stars);
            }
            
            if (Unlocks.ContainsKey(level.name))
            {
                Unlocks[level.name] = level.isLocked;
            }
            else
            {
                Unlocks.Add(level.name, level.isLocked);
            }
        }
        public static void SaveSettingsData()
        {
            ClipSliderValue = ClipSpeedSlider.SpeedSlider.value;
            SelectedLocaleIndex = LanguageToggler.GetCurrentLocaleIndex();
        }
            
    }
}