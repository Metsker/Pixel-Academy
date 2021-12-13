using System;
using System.Globalization;
using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.SharedOverall.Animating;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Settings
{
    public class ClipSpeedSlider : MonoBehaviour
    {
        [SerializeField] private ClipPlaying clipPlaying;
        [SerializeField] private TextMeshProUGUI animationSpeed;
        
        public static Slider slider { get; private set; }

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void Start()
        {
            gameObject.SetActive(GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play);
            slider.onValueChanged.AddListener(_ => ValueChangeCheck());
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        private void ValueChangeCheck()
        {
            clipPlaying.animator.speed = Mathf.Lerp(0.1f,2,slider.value);
            ResultCalculator.RecoveryTiming = Mathf.Lerp(400, 20, slider.value);
            animationSpeed.SetText(Math.Round(clipPlaying.animator.speed,1).ToString(CultureInfo.CurrentCulture));
        }
    }
}