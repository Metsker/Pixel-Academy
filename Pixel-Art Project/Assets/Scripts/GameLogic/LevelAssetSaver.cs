using System.Collections.Generic;
using System.Linq;
using MapEditor.PresetSettings;
using MapEditor.Recording;
using UnityEditor;
using UnityEngine;

namespace AnimPlaying
{
    public static class LevelAssetSaver
    {
        public static void CreateMyAsset(List<Color> pixelList, List<GameObject> presetList)
        {
            var name = AnimClipLoader.AnimationClips[AnimClipSelector.ClipNumber].name;
            LevelScriptableObject asset = ScriptableObject.CreateInstance<LevelScriptableObject>();
            asset.statesList = pixelList;
            asset.sideLenght = DrawingPanelCreator.sizeFieldHandler.size;
            asset.colorPresetList = presetList;
            AssetDatabase.CreateAsset(asset, "Assets/Resources/LessonLevels/" + name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
