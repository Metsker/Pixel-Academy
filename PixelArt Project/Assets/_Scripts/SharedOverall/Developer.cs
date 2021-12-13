#if UNITY_EDITOR
using System;
using _Scripts.SharedOverall.Saving;
using _Scripts.SharedOverall.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall
{
    public static class Developer
    {
        public static event Action UpdateUI;

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
        
        [MenuItem("Developer/Get 25 Hint Tokens")]
        public static void GetHintTokens()
        {
            PlayerPrefs.SetInt("HintTokens", PlayerPrefs.GetInt("HintTokens", 3)+25);
            PlayerPrefs.Save();
            UpdateUI?.Invoke();
        }
        
        [MenuItem("Developer/Spend Hint Tokens")]
        public static void SpendHintTokens()
        {
            PlayerPrefs.SetInt("HintTokens", 0);
            PlayerPrefs.Save();
            UpdateUI?.Invoke();
        }
    }
}
#endif