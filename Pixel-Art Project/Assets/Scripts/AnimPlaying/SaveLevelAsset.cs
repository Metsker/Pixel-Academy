using System.Collections.Generic;
using System.Linq;
using MapEditor.PresetSettings;
using MapEditor.Recording;
using UnityEditor;
using UnityEngine;

namespace AnimPlaying
{
    public static class SaveLevelAsset
    {
        public static void CreateMyAsset(List<Color> levelList)
        {
            var name = AnimClipLoader.AnimationClips[AnimClipSelector.ClipNumber].name;
            SaveLevelScriptableObject asset = ScriptableObject.CreateInstance<SaveLevelScriptableObject>();
            asset.statesList = levelList;
            asset.sideLenght = DrawingPanelCreator.DrawingSizeField.size;
            asset.usedColors = levelList.Distinct().ToList();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/LessonLevels/" + name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
