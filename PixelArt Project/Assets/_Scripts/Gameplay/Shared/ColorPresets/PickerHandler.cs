using System.Threading.Tasks;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.SharedOverall.Utility;
using HSVPicker;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.ColorPresets
{
    public class PickerHandler : MonoBehaviour
    {
        [SerializeField] private ColorPicker picker;
        private static ColorPicker _picker;
        private static RectTransform _rectTransform;
        private static float _startPos;
        private static float _endPos = 1;
        private static bool _isAnimating;

        private void Awake()
        {
            _picker = picker;
            _rectTransform = _picker.GetComponent<RectTransform>();
        }

        public static void EnablePicker(UnityAction<Color> pickerAction, Image image)
        {
            if (!IsPickerActive())
            {
                _picker.gameObject.SetActive(true);
                PickerAnimation();
            }
            foreach (var ps in ColorPresetSpawner.ColorPresets)
            {
                _picker.onValueChanged.RemoveListener(ps.PickerAction);
            }
            _picker.CurrentColor = image.color;
            _picker.onValueChanged.AddListener(pickerAction);
        }

        public static void DisablePicker()
        {
            if (!IsPickerActive()) return;
            PickerAnimation();
        }
        public static bool IsPickerActive()
        {
            return GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play && _picker.gameObject.activeSelf;
        }

        public static void RandomizeColor()
        {
            var c = ColorRandomizer.GetRandomColor();
            _picker.CurrentColor = new Color(c.r, c.g, c.b, _picker.CurrentColor.a);
        }

        private static async void PickerAnimation()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            
            for (float i = 0; i < 1; i += Time.deltaTime*2)
            {
                _rectTransform.pivot = new Vector2
                    (Mathf.Lerp(_startPos, _endPos, EasingSmoothSquared(i)), _rectTransform.pivot.y);
                await Task.Yield();
            }
            var cash = _startPos;
            _startPos = _endPos;
            _endPos = cash;
            if (_rectTransform.pivot.x < 0.5f)
            {
                _picker.gameObject.SetActive(false);
            }
            _isAnimating = false;
        }

        private static float EasingSmoothSquared(float x)
        {
            return x < 0.5f ? x * x * 2 : 1 - (1 - x) * (1 - x) * 2;
        }
    }
}