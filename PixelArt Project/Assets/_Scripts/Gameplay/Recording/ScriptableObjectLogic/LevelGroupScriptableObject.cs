using System.Collections.Generic;
using System.IO;
using _Scripts.Gameplay.Recording.UI;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    [CreateAssetMenu(fileName = "Group", menuName = "ScriptableObjects/LevelGroup", order = 1)]
    public class LevelGroupScriptableObject : ScriptableObject
    {
        public const string FolderName = "LevelGroupObjects";
        
        public List<LevelScriptableObject> levels = new();
        
#if UNITY_EDITOR    
        public static LevelGroupScriptableObject GetLevelGroupScrObj()
        {
            var path = $"Assets/Resources/{FolderName}/{LevelGroupManager.selectedGroupType.ToString()}.asset";
            if (!File.Exists(path))
            {
                var asset = CreateInstance<LevelGroupScriptableObject>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }
            return Resources.Load<LevelGroupScriptableObject>($"{FolderName}/{LevelGroupManager.selectedGroupType.ToString()}");
        }
#endif
    }
}
