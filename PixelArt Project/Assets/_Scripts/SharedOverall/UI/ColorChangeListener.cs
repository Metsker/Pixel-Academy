using _Scripts.Gameplay.Shared.ColorPresets;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.UI
{
    public class ColorChangeListener : MonoBehaviour
    {
        private Image _image;
    
        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }
        protected void OnEnable()
        {
            ColorPreset.ColorChange += OnColorChange;
        }
        protected void OnDisable()
        {
            ColorPreset.ColorChange -= OnColorChange;
        }
        protected void OnColorChange()
        {
            _image.color = ColorPreset.GetColor() == null ? Color.black : ColorPreset.GetColor().Value;
        }
    }
}
