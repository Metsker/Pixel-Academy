using System;
using System.Collections;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.SharedOverall.ColorPresets.PickerHandler;

namespace _Scripts.SharedOverall.ColorPresets
{
    public class ColorPreset : PencilTool, IPointerClickHandler
    {
        [HideInInspector]
        public Image image;
        public ColorPresetStruct PresetStruct { get; private set; }
        public UnityAction<Color> pickerAction;
        public static event Action StartLevel;
        
        private int _tap;
        private const int TapCount = 2;
        private const float PickerDelay = 0.25f;

        private new void Awake()
        {
            base.Awake();
            image = GetComponent<Image>();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            pickerAction = color =>
            {
                image.color = color;
                SetColor(color);
            };
        }

        public new void OnPointerClick(PointerEventData eventData)
        {
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Paint:
                    if (IsPickerActive() && IsSelected())
                    {
                        DisablePicker();
                        break;
                    }
                    if (!IsPickerInvoked()) { break; } else { return; }
                case GameModeManager.GameMode.Record when 
                    GameStateManager.CurrentGameState != GameStateManager.GameState.Recording:
                    if (IsPickerActive() && IsSelected())
                    {
                        DisablePicker();
                        break;
                    }
                    if (!IsPickerInvoked()) { break; } else { return; }
                case GameModeManager.GameMode.Record when GameStateManager.CurrentGameState == GameStateManager.GameState.Recording && ColorPresetSpawner.GetSelected() != null && ColorPresetSpawner.GetSelected() == this :
                    return;
                case GameModeManager.GameMode.Play when
                    GameStateManager.CurrentGameState == GameStateManager.GameState.Animating:
                    StartLevel?.Invoke();
                    break;
            }
            base.OnPointerClick(eventData);
            SetColor(image.color);
            if (!IsPickerActive()) return;
            EnablePicker(pickerAction, image);
        }

        private bool IsPickerInvoked()
        {
            _tap++;
            StartCoroutine(Timer());
            if (_tap != TapCount) return false;
            _tap = 0;
            EnablePicker(pickerAction, image);
            return true;
        }
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(PickerDelay);
            _tap = 0;
        }
        public void AssignStruct()
        {
            PresetStruct = new ColorPresetStruct(image.color, transform.parent.gameObject.name);
        }
    }
}
