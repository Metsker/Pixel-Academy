using System;
using System.Collections;
using _Scripts.SharedOverall.Saving;
using UnityEngine;
using UnityEngine.Localization;
using static UnityEngine.Localization.Settings.LocalizationSettings;

namespace _Scripts.SharedOverall
{
    public class LanguageManager : MonoBehaviour
    {
        public static LocaleLanguages CurrentLanguage;
        public enum LocaleLanguages
        {
            English,
            Russian
        }
        private IEnumerator Start()
        {
            yield return InitializationOperation;
            
            switch (SaveData.SelectedLocaleIndex)
            {
                case null when Enum.TryParse(Application.systemLanguage.ToString(), out CurrentLanguage):
                    SelectedLocale = GetCurrentLanguageLocale();
                    break;
                case null when !Enum.TryParse(Application.systemLanguage.ToString(), out CurrentLanguage):
                    CurrentLanguage = LocaleLanguages.English;
                    SelectedLocale = GetCurrentLanguageLocale();
                    break;
                default:
                    CurrentLanguage = (LocaleLanguages)SaveData.SelectedLocaleIndex;
                    SelectedLocale = GetCurrentLanguageLocale();
                    break;
            }
        }

        private static Locale GetCurrentLanguageLocale()
        {
            return AvailableLocales.Locales[(int)CurrentLanguage];
        }

        public static bool IsLanguageInitialized()
        {
            return SelectedLocale == GetCurrentLanguageLocale();
        }
    }
}