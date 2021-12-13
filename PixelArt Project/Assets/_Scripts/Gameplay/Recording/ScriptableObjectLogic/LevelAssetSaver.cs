#if (UNITY_EDITOR)
using System.Collections.Generic;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.Gameplay.Recording.Recording;
using _Scripts.Gameplay.Recording.UI;
using UnityEditor;
using UnityEngine;
using static _Scripts.SharedOverall.DrawingPanel.DrawingPanelCreator;

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    public static class LevelAssetSaver
    {
        private const string LevelPath = "Assets/Resources/Levels";
        
        public static void CreateStageAsset(List<Color> pixelList, List<ColorPresetStruct> colorList,
            AnimationClip clip, List<float> audioClickTimings, List<float> audioToolTimings)
        {
            var name = ClipListLoader.GetCurrentClip().name;
            var partLength = Recorder.Part.ToString().Length;
            var folderName = name.Remove(name.Length - partLength - 1);
            var asset = ScriptableObject.CreateInstance<StageScriptableObject>();
            asset.animationClip = clip;
            asset.pixelList = pixelList;
            asset.colorPresetStruct = colorList;
            asset.audioClickTimings = audioClickTimings;
            asset.audioToolTimings = audioToolTimings;
            AssetDatabase.CreateAsset(asset, $"{LevelPath}/{folderName}/Data/{name}.asset");
            AssetDatabase.SaveAssets();
        }

        public static void CreateLevelAsset()
        {
            var name = ClipListLoader.GetCurrentClip().name;
            var partLength = Recorder.Part.ToString().Length;
            var folderName = name.Remove(name.Length - partLength - 1);
            var asset = ScriptableObject.CreateInstance<LevelScriptableObject>();
            asset.xLenght = X;
            asset.yLenght = Y;
            asset.difficulty = DifficultyCalculator.Calculate(X * Y);
            asset.previewSprite = Resources.Load<Sprite>($"Levels/{folderName}/{folderName}");
            asset.groupType = LevelGroupManager.selectedGroupType;
            asset.stageScriptableObjects = new List<StageScriptableObject>();
            for (var i = 0; i < Recorder.Part + 1; i++)
            {
                asset.stageScriptableObjects.Add(Resources.Load<StageScriptableObject>(
                    $"Levels/{folderName}/Data/{folderName}_{i}"));
            }
    
            var levelPath = $"{LevelPath}/{folderName}/{folderName}.asset";
            AssetDatabase.CreateAsset(asset, levelPath);
            LevelGroupScriptableObject.GetLevelGroupScrObj().levels.Add(
                Resources.Load<LevelScriptableObject>($"Levels/{folderName}/{folderName}"));
            
            var nlg = Resources.Load<LevelGroupScriptableObject>("LevelGroupObjects/News").levels;
            nlg.Insert(0,asset);
            if (nlg.Count > 6)
            {
                nlg.RemoveAt(nlg.Count-1);
            }

            EditorUtility.SetDirty(LevelGroupScriptableObject.GetLevelGroupScrObj());
            AssetDatabase.SaveAssets();
        }
    }
}
#endif