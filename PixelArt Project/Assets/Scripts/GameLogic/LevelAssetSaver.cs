using System.Collections.Generic;
using AnimPlaying;
using Gameplay;
using MapEditor.ColorPresets;
using MapEditor.PresetSettings;
using UnityEditor;
using UnityEngine;

namespace GameLogic
{
    public static class LevelAssetSaver
    {
        public static void CreateMyAsset(List<Color> pixelList, List<ColorPresetStruct> presetList)
        {
            var name = ClipListLoader.AnimationClips[AnimUIUpdater.ClipNumber].name;
            LevelScriptableObject asset = ScriptableObject.CreateInstance<LevelScriptableObject>();
            asset.statesList = pixelList;
            asset.sideLenght = DrawingPanelCreator.sizeFieldHandler.size;
            presetList.Reverse();
            asset.colorPresetStruct = presetList;
            asset.firstColor = ClickOnPixel.firstPixelColor;
            AssetDatabase.CreateAsset(asset, "Assets/Resources/LessonLevels/" + name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
