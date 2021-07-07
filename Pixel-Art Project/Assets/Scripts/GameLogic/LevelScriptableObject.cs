using System.Collections.Generic;
using UnityEngine;

namespace MapEditor.Recording
{
    public class LevelScriptableObject : ScriptableObject
    {
        public List<Color> statesList;
        public int sideLenght;
        public List<GameObject> colorPresetList;
        public Color firstPixel;
    }
}
