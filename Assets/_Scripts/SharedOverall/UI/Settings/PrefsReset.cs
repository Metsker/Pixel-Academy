using System;
using UnityEngine;

namespace _Scripts.SharedOverall.UI.Settings
{
    public class PrefsReset : MonoBehaviour
    {
        public void ResetClick()
        {
            PlayerPrefs.SetInt("ClearWarning", 0);
            PlayerPrefs.SetInt("ClipWarning", 0);
            PlayerPrefs.SetInt("ClipHintWarning", 0);
            PlayerPrefs.SetInt("OpaqueHintWarning", 0);
            PlayerPrefs.SetInt("DrawingTipWarning", 0);
            PlayerPrefs.SetInt("ExitWarning", 0);
            PlayerPrefs.Save();
        }
    }
}