using UnityEngine;

namespace _Scripts.Gameplay.Recording.ColorPresets
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