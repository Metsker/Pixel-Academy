using _Scripts.SharedOverall.ColorPresets;
using UnityEngine;

namespace _Scripts.SharedOverall.Utility
{
    public class RandomPickerColor : MonoBehaviour
    {
        public void Click()
        {
            PickerHandler.RandomizeColor();
        }
    }
}