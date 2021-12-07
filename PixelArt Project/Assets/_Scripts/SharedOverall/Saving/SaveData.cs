using System;
using System.Collections.Generic;
using _Scripts.GeneralLogic.Data;

namespace _Scripts.GeneralLogic.Saving
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