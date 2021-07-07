using System.Collections.Generic;
using MapEditor.ColorPresets;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class LevelScriptableObject : ScriptableObject
    {
        public List<Color> statesList;
        public List<ColorPresetStruct> colorPresetStruct;
        public int sideLenght;
        public Color firstColor;
    }
}
