using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Animating
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField] private Slider slider; 
        private static Image _sliderImage;
        public static Slider Slider { get; private set; }
        public static readonly Color AnimationColor = new(1, 0.8078431f, 0.3607843f, 1);
        public static readonly Color CorrectionColor = Color.red;

        private void Awake()
        {
            Slider = slider;
            _sliderImage = Slider.fillRect.GetComponent<Image>();
        }

        public static void SetProgressColor(Color c)
        {
            _sliderImage.color = c;
        }
        public static void ToggleSliderState(bool state)
        {
            if (Slider.gameObject.activeSelf == state) return;
            Slider.value = 0;
            Slider.gameObject.SetActive(state);
        }

        public static bool IsComplete()
        {
            return Slider.normalizedValue > 0.99f;
        }
    }
}