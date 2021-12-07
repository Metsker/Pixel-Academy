using System.Collections.Generic;
using _Scripts.EditorMod.ScriptableObjectLogic;

namespace _Scripts.GeneralLogic.Data
{
    public static class ScriptableObjectDataSaver
    {
        public static Dictionary<string, int> Stars = new();
        public static Dictionary<string, bool> Unlocks = new();

        public static void SaveData(LevelScriptableObject level)
        {
            if (Stars.ContainsKey(level.name))
            {
                Stars[level.name] = level.stars;
            }
            else
            {
                Stars.Add(level.name, level.stars);
            }
            //
            if (Unlocks.ContainsKey(level.name))
            {
                Unlocks[level.name] = level.isLocked;
            }
            else
            {
                Unlocks.Add(level.name, level.isLocked);
            }
        }
    }
}