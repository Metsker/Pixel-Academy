using System.Collections.Generic;
using System.IO;
using _Scripts.Gameplay.Recording.UI;
using _Scripts.SharedOverall;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    [CreateAssetMenu(fileName = "Group", menuName = "ScriptableObjects/LevelGroup", order = 1)]
    public class LevelGroupScriptableObject : ScriptableObject
    {
        private const string FolderName = "LevelGroupObjects";
        public LevelGroupsManager.GroupType groupType;
        
        public List<LevelScriptableObject> levels = new();
        
#if UNITY_EDITOR 
        public static LevelGroupScriptableObject GetCurrentLevelGroupScrObj()
        {
            var path = $"Assets/Resources/{FolderName}/{LevelGroupsManager.SelectedGroupType.ToString()}.asset";
            if (!File.Exists(path))
            {
                var asset = CreateInstance<LevelGroupScriptableObject>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups.Add(asset);
            }
            return LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups.Find((g) => g.groupType == LevelGroupsManager.SelectedGroupType);
        }
#endif
    }
}
