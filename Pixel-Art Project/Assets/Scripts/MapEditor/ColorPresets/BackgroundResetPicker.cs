using HSVPicker;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapEditor.ColorPresets
{
    public class BackgroundResetPicker : MonoBehaviour, IPointerClickHandler
    {
        private ColorPicker _picker;

        private void Awake()
        {
            _picker = FindObjectOfType<ColorPresetCreateButton>().picker;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_picker.gameObject.activeSelf)
            {
                _picker.gameObject.SetActive(false);
            }
        }
    }
}
