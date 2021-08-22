using UnityEngine;

namespace EditorMod.ColorPresets
{
    [System.Serializable]
    public struct ColorPresetStruct
    {
        public Color color;
        public string name;

        public ColorPresetStruct(Color c, string n)
        {
            color = c;
            name = n;
        }
    }
}