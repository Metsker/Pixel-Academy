using System.Collections.Generic;
using EditorMod.ColorPresets;
using UnityEngine;

namespace EditorMod.ScriptableObjectLogic
{
    public class StageScriptableObject : ScriptableObject
    {
        public AnimationClip animationClip;
        public List<Color> statesList;
        public List<ColorPresetStruct> colorPresetStruct;
    }
}
