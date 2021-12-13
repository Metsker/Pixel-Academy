using UnityEngine;

namespace _Scripts.SharedOverall.Utility
{
    public static class ColorRandomizer
    {
        public static Color GetRandomColor()
        {
            return new (
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f));
        }
    }
}