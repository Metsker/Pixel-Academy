using System;
using System.Globalization;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.SharedOverall.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.UI.Settings
{
    public class ClipSpeedSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI animationSpeedTxt;
        private ClipPlaying _clipPlaying;
        public static Slider SpeedSlider { get; private set; }
        
        public const float ClipMin = 0.5f;
        public const float ClipMax = 2;
        public const float ClipDefault = 1;
        public const float CorrectionMin = 0.4f;
        public const float CorrectionMax = 0.1f;

        private void Awake()
        {
            SpeedSlider = GetComponent<Slider>();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play)
            {
                _clipPlaying = FindObjectOfType<ClipPlaying>();
            }
            if (SaveData.ClipSliderValue == null) return;
            SpeedSlider.value = (float)SaveData.ClipSliderValue;
            SetUI();
        }

        public void Start()
        {
            SpeedSlider.onValueChanged.AddListener(_ => ValueChangeCheck());
        }

        private void ValueChangeCheck()
        {
            SetUI();
            if (_clipPlaying == null) return;
            _clipPlaying.animator.speed = Mathf.Lerp(ClipMin,ClipMax,SpeedSlider.value);
            ResultCorrector.RecoveryTiming = Mathf.Lerp(CorrectionMin,CorrectionMax, SpeedSlider.value);
            if (GameStateManager.CurrentGameState != GameStateManager.GameState.Correcting) return;
            ResultCorrector.InitCorrectionDuration();

        }
        
        private void SetUI()
        {
            var speed = Mathf.Lerp(ClipMin,ClipMax,SpeedSlider.value);
            animationSpeedTxt.SetText(Math.Round(speed, 1).ToString(CultureInfo.CurrentCulture) + "x");
        }

        public static float GetSavedAnimatorSpeed()
        {
            var v = SaveData.ClipSliderValue switch
            {
                null => ClipDefault,
                _ => (float) SaveData.ClipSliderValue
            };
            return Mathf.Lerp(ClipMin,ClipMax,v);
        }
    }
}