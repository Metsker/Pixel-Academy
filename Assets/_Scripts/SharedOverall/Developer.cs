#if UNITY_EDITOR
using _Scripts.SharedOverall.Saving;
using UnityEditor;
using UnityEngine;

namespace _Scripts.SharedOverall
{
    public static class Developer
    {
        [MenuItem("Developer/Reset Prefs")]
        public static void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        
        [MenuItem("Developer/Delete Save File")]
        public static void DeleteSaveFile()
        {
            SaveSystem.DeleteSaveFile();
        }
        
        [MenuItem("Developer/Get 25 Coins")]
        public static void GetCoins()
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0)+25);
            PlayerPrefs.Save();
            CoinsUI.UpdateCoinsUI();
        }
        
        [MenuItem("Developer/Spend Coins")]
        public static void SpendCoins()
        {
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.Save();
            CoinsUI.UpdateCoinsUI();
        }
    }
}
#endif