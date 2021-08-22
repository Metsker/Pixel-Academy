using System;
using EditorMod.ColorPresets;
using GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.ColorPresets
{
    public class ColorPreset : PencilTool, IPointerClickHandler
    {
        private Image _image;
        public ColorPresetStruct PresetStruct { get; private set; }
        public static event Action<UnityAction<Color>, Image> EnablePicker;
        public static event Action DisablePicker;
        public UnityAction<Color> pickerAction;

        private new void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play) return;
            pickerAction = color =>
            {
                _image.color = color;
                SetColor(color);
            };
        }

        public new void OnPointerClick(PointerEventData eventData)
        {
            if (GameModManager.CurrentGameMod != GameModManager.GameMod.Play 
                && GameStateManager.CurrentGameState != GameStateManager.GameState.Recording 
                && eventData.clickCount == 2)
            {
                DisablePicker?.Invoke();
                EnablePicker?.Invoke(pickerAction, _image);
                return;
            }
            base.OnPointerClick(eventData);
            SetColor(_image.color);
        }

        public void AssignStruct()
        {
            PresetStruct = new ColorPresetStruct(_image.color, transform.parent.gameObject.name);
        }
    }
}
