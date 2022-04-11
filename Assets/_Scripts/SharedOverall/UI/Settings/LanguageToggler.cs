using System;
using static UnityEngine.Localization.Settings.LocalizationSettings;

namespace _Scripts.SharedOverall.UI.Settings
{
    public class LanguageToggler : Setting
    {
        public static event Action UpdateUI;
        private void Start()
        {
            image.sprite = sprites[GetCurrentLocaleIndex()];
        }

        public override void ToggleClick()
        {
            if (!InitializationOperation.IsDone) return;
            SelectedLocale = AvailableLocales.Locales[GetNextLocaleIndex()];
            image.sprite = sprites[GetCurrentLocaleIndex()];
            LanguageManager.CurrentLanguage = (LanguageManager.LocaleLanguages)GetCurrentLocaleIndex();
            UpdateUI?.Invoke();
        }

        private static int GetNextLocaleIndex()
        {
            return SelectedLocale.SortOrder == AvailableLocales.Locales.Count - 1 ?
                0 : SelectedLocale.SortOrder + 1;
        }

        public static int GetCurrentLocaleIndex()
        {
            return SelectedLocale.SortOrder;
        }
    }
}