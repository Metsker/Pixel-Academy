using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace _Scripts.SharedOverall.Saving
{
    public static class SaveSystem
    {
        private static readonly string Path = Application.persistentDataPath + "/save.pixel";
        
        public static void SaveDataToFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Path, FileMode.Create);
            SaveDataObject saveDataObject = new SaveDataObject();
            formatter.Serialize(stream, saveDataObject);
            stream.Close();
        }
        
        private static SaveDataObject LoadData()
        {
            if (File.Exists(Path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Path, FileMode.Open);
                SaveDataObject saveDataObject = formatter.Deserialize(stream) as SaveDataObject;
                stream.Close();
                return saveDataObject;
            }
            Debug.LogError("Save file not found in " + Path);
            return null;
        }
        
        public static void SetData()
        {
            var save = LoadData();
            if (save == null) return;
            
            SaveData.Stars = save.Stars;
            SaveData.Unlocks = save.Unlocks;

            SaveData.ClipSliderValue = save.clipSpeedValue;
            SaveData.SelectedLocaleIndex = save.selectedLocaleIndex;

            foreach (var group in LevelGroupsLoader.levelGroupsLoader.levelGroups)
            {
                foreach (var level in group.levels.Where(l => l != null))
                {
                    if (SaveData.Stars.ContainsKey(level.name))
                    {
                        level.stars = SaveData.Stars[level.name];
                    }
                    if (SaveData.Unlocks.ContainsKey(level.name))
                    {
                        level.isLocked = SaveData.Unlocks[level.name];
                    } 
                }
            }
        }
        public static void DeleteSaveFile()
        {
            File.Delete(Path);
        }
    }
}