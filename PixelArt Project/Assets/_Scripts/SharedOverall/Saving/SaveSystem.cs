using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using _Scripts.EditorMod.ScriptableObjectLogic;
using _Scripts.EditorMod.UI;
using _Scripts.GameplayMod.Resulting;
using _Scripts.GeneralLogic.Data;
using UnityEngine;
using static _Scripts.EditorMod.ScriptableObjectLogic.LevelGroupScriptableObject;

namespace _Scripts.GeneralLogic.Saving
{
    public static class SaveSystem
    {
        private static readonly string Path = Application.persistentDataPath + "/save.pixel";
        
        public static void SaveData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Path, FileMode.Create);
            SavedData savedData = new SavedData();
            formatter.Serialize(stream, savedData);
            stream.Close();
        }
        
        private static SavedData LoadData()
        {
            if (File.Exists(Path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Path, FileMode.Open);
                SavedData savedData = formatter.Deserialize(stream) as SavedData;
                stream.Close();
                return savedData;
            }
            Debug.LogError("Save file not found in " + Path);
            return null;
        }
        
        public static void SetData()
        {
            var save = LoadData();
            if (save == null) return;
            
            ScriptableObjectDataSaver.Stars = save.Stars;
            ScriptableObjectDataSaver.Unlocks = save.Unlocks;

            for (var i = 0; i < Enum.GetNames(typeof(LevelGroupManager.GroupType)).Length; i++)
            {
                foreach (var level in Resources.Load<LevelGroupScriptableObject>(
                    $"{FolderName}/{Enum.GetNames(typeof(LevelGroupManager.GroupType))[i]}").levels.Where(l => l != null))
                {
                    if (ScriptableObjectDataSaver.Stars.ContainsKey(level.name))
                    {
                        level.stars = ScriptableObjectDataSaver.Stars[level.name];
                    }
                    if (ScriptableObjectDataSaver.Unlocks.ContainsKey(level.name))
                    {
                        level.isLocked = ScriptableObjectDataSaver.Unlocks[level.name];
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