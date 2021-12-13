using UnityEngine.Localization.Settings;

namespace _Scripts.SharedOverall.Settings.LanguageToggler
{
    public class LanguageToggler : Setting
    {
        public override void ToggleClick()
        {
            if (!LocalizationSettings.InitializationOperation.IsDone) return;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[GetNextLocaleIndex()];
            image.sprite = sprites[GetNextLocaleIndex()]; //индексы как в locales
        }

        private static int GetNextLocaleIndex()
        {
            return LocalizationSettings.SelectedLocale.SortOrder ==
                   LocalizationSettings.AvailableLocales.Locales.Count - 1
                ? LocalizationSettings.SelectedLocale.SortOrder + 1
                : 0;
        }

        public static int GetCurrentLocaleIndex()
        {
            return LocalizationSettings.SelectedLocale.SortOrder;
        }
    }
}