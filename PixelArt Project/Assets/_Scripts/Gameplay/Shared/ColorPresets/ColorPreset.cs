using System;
using System.Collections;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.SharedOverall.ColorPresets.PickerHandler;

namespace _Scripts.Gameplay.Shared.ColorPresets
{
    public class ColorPreset: SelectableColor, IPointerClickHandler
    {
        public Image colorImage;
        public ColorPresetStruct PresetStruct { get; private set; }
        public UnityAction<Color> PickerAction;

        private int _tap;
        private const int TapCount = 2;
        private const float PickerDelay = 0.25f;
        
        private static Color? _pickedColor;
        private readonly Color _deselectedColor = new (1,1,1,0);
        public static event Action ColorChange;

        private new void Awake()
        {
            base.Awake();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            PickerAction = color =>
            {
                SetImageColor(color);
                SetColor(color);
            };
        }
        public void OnPointerClick(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording || !ColorUtility.TryParseHtmlString(GUIUtility.systemCopyBuffer,
                    out var c)) return;
                SetImageColor(c);
                SelectColor(false);
                return;
            }
#endif
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Paint:
                    if (IsPickerActive() && IsSelected())
                    {
                        DisablePicker();
                        break;
                    }
                    if (!IsPickerInvoked()) break; else return;
                case GameModeManager.GameMode.Record when 
                    GameStateManager.CurrentGameState != GameStateManager.GameState.Recording:
                    if (IsPickerActive() && IsSelected())
                    {
                        DisablePicker();
                        break;
                    }
                    if (!IsPickerInvoked()) break; else return;
                case GameModeManager.GameMode.Record when GameStateManager.CurrentGameState == GameStateManager.GameState.Recording && ColorPresetSpawner.GetSelected() != null && ColorPresetSpawner.GetSelected() == this :
                    return;
            }
            SelectColor(true);
            if (!IsPickerActive()) return;
            EnablePicker(PickerAction, colorImage);
        }
        public void SelectColor(bool checkStates)
        {
            switch (checkStates)
            {
                case true:
                    ClickEvent();
                    break;
                default:
                    PlayAnimation();
                    break;
            }
            SelectWithoutAnimation();
            SetColor(colorImage.color);
        }
        public static void SetColor(Color? c)
        {
            _pickedColor = c;
            ColorChange?.Invoke();
        }
        public static Color? GetColor()
        {
            return _pickedColor;
        }
        public void SetImageColor(Color c)
        {
            colorImage.color = c;
        }
        public Color GetImageColor()
        {
            return colorImage.color;
        }
        public override Color GetDeselectedColor()
        {
            var color = GetColor() ?? GetImageColor();
            if (color.a < 0.3f) return base.GetDeselectedColor();
            return color.CompareRGB(Color.white) ? base.GetDeselectedColor() : _deselectedColor; 
        }
        private bool IsPickerInvoked()
        {
            _tap++;
            StartCoroutine(Timer());
            if (_tap != TapCount) return false;
            _tap = 0;
            EnablePicker(PickerAction, colorImage);
            return true;
        }
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(PickerDelay);
            _tap = 0;
        }
        public void AssignStruct()
        {
            PresetStruct = new ColorPresetStruct(colorImage.color, transform.parent.gameObject.name);
        }
    }
}
