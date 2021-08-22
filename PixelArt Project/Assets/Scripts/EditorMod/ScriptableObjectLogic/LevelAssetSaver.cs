using System.Collections.Generic;
using System.Threading.Tasks;
using EditorMod.Animating;
using EditorMod.ColorPresets;
using UnityEditor;
using UnityEngine;
using static GeneralLogic.DrawingPanel.DrawingPanelCreator;

namespace EditorMod.ScriptableObjectLogic
{
    public static class LevelAssetSaver
    {
        public static void CreateStageAsset(List<Color> pixelList, List<ColorPresetStruct> colorList,
            AnimationClip clip)
        {
            var name = ClipListLoader.AnimationClips[ClipListLoader.ClipNumber].name;
            StageScriptableObject asset = ScriptableObject.CreateInstance<StageScriptableObject>();
            asset.animationClip = clip;
            asset.statesList = pixelList;
            asset.colorPresetStruct = colorList;
            AssetDatabase.CreateAsset(asset, $"Assets/Resources/Lessons/Levels/{name}.asset");
            AssetDatabase.SaveAssets();
        }

        public static void CreateLevelAsset()
        {
            var name = ClipListLoader.AnimationClips[ClipListLoader.ClipNumber].name;
            var asset = ScriptableObject.CreateInstance<LevelScriptableObject>();
            asset.xLenght = X;
            asset.yLenght = Y;
            asset.difficulty = DifficultyCalculator.Calculate(X * Y);
            asset.previewSprite = Resources.Load<Sprite>($"Lessons/Snapshots/{name}");
        }
    }
}
