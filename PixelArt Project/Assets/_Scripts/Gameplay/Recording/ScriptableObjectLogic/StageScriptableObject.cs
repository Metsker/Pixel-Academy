using System.Collections.Generic;
using _Scripts.EditorMod.ColorPresets;
using UnityEngine;

namespace _Scripts.EditorMod.ScriptableObjectLogic
{
    public class StageScriptableObject : ScriptableObject
    {
        public AnimationClip animationClip;
        public List<float> audioClickTimings;
        public List<float> audioToolTimings;
        public List<Color> pixelList;
        public List<ColorPresetStruct> colorPresetStruct;
    }
}
