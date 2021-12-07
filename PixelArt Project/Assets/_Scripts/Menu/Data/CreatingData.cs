using System.Collections;
using _Scripts.GeneralLogic.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace _Scripts.GeneralLogic.Menu.Data
{
    public class CreatingData : MonoBehaviour
    {
        [Header("Dependencies")]
        public GameObject categoryInstance;
        public GameObject levelPanel;
        public Sprite filledStar;
        public Sprite unfilledStar;
        public Sprite unlockedShape;
        public Sprite lockedShape;
        public Color lockedColor;
        public Color completedColor;
        public TextMeshProUGUI label;

        private void Awake()
        {
            SaveSystem.SetData();
        }
        public IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            if (LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales
                [Application.systemLanguage == SystemLanguage.Russian ? 0 : 1])
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales
                    [Application.systemLanguage == SystemLanguage.Russian ? 0 : 1];
            }
        }
    }
}
