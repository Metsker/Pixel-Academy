using System;
using _Scripts.SharedOverall.Saving;
using UnityEngine;

namespace _Scripts.SharedOverall.UI.Settings
{
    public class SettingsUI : MonoBehaviour
    {
        public static event Action SwitchBlur;
        
        public void CloseSettings()
        {
            SwitchBlur?.Invoke();
            SaveData.SaveSettingsData();
        }
    }
}