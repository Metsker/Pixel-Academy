using System;
using System.Collections.Generic;
using _Scripts.SharedOverall.Data;

namespace _Scripts.SharedOverall.Saving
{
    [Serializable]
    public class SavedData
    {
        public Dictionary<string, int> Stars;
        public Dictionary<string, bool> Unlocks;

        public SavedData()
        {
            Stars = ScriptableObjectDataSaver.Stars;
            Unlocks = ScriptableObjectDataSaver.Unlocks;
        }
    }
}