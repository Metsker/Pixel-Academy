#if (UNITY_EDITOR)
using System.Collections.Generic;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.Gameplay.Recording.Recording;
using _Scripts.Gameplay.Recording.UI;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.SharedOverall;
using UnityEditor;
using UnityEngine;
using static _Scripts.SharedOverall.DrawingPanel.DrawingPanelCreator;

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    public static class LevelAssetSaver
    {
        public const string LevelPath = "Assets/Resources/Levels";
        
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

            if (!LevelGroupsLoader.TryFindLevelInCurrentGroup(folderName, out var level))
            {
                if (!LevelGroupsLoader.TryFindLevelInOtherGroups(folderName, out level))
                {
                    return;
                }
            }
            
            if (level.stageScriptableObjects.Count > Recorder.Part)
            {
                level.stageScriptableObjects[Recorder.Part] = asset;
            }
            else
            {
                level.stageScriptableObjects.Add(asset);
            }
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
            asset.groupType = LevelGroupsManager.SelectedGroupType;
            asset.stageScriptableObjects = new List<StageScriptableObject>();
            for (var i = 0; i < Recorder.Part + 1; i++)
            {
                asset.stageScriptableObjects.Add(Resources.Load<StageScriptableObject>(
                    $"Levels/{folderName}/Data/{folderName}_{i}"));
            }
            var levelPath = $"{LevelPath}/{folderName}/{folderName}.asset";
            AssetDatabase.CreateAsset(asset, levelPath);
            LevelGroupScriptableObject.GetCurrentLevelGroupScrObj().levels.Add(
                asset);
            EditorUtility.SetDirty(LevelGroupScriptableObject.GetCurrentLevelGroupScrObj());
            AssetDatabase.SaveAssets();
        }
    }
}
#endif