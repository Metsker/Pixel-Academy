using System.Collections.Generic;
using UnityEngine;

namespace MapEditor.Recording
{
    public class SaveLevelScriptableObject : ScriptableObject
    {
        public List<Color> statesList;
        public int sideLenght;
        public List<Color> usedColors;
    }
}
