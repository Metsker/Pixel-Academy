using System;
using System.Collections.Generic;

namespace _Scripts.SharedOverall.Saving
{
    [Serializable]
    public class SaveDataObject
    {
        public Dictionary<string, int> Stars;
        public Dictionary<string, bool> Unlocks;

        public float? clipSpeedValue;
        public int? selectedLocaleIndex;

        public SaveDataObject()
        {
            Stars = SaveData.Stars;
            Unlocks = SaveData.Unlocks;

            clipSpeedValue = SaveData.ClipSliderValue;
            selectedLocaleIndex = SaveData.SelectedLocaleIndex;
        }
    }
}