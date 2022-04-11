using System;
using _Scripts.SharedOverall.Saving;

namespace _Scripts.SharedOverall.UI.Settings
{
    public class SettingsUI : UIPanel
    { 
        public static event Action<bool> SwitchBlur;
        public void OpenUI()
        {
            SwitchBlur?.Invoke(true);
            gameObject.SetActive(true);
        }

        public override void CloseUI()
        {
            if(!gameObject.activeSelf) return;
            SwitchBlur?.Invoke(false);
            gameObject.SetActive(false);
            SaveData.SaveSettingsData();
            SaveSystem.SaveDataToFile();
        }
    }
}