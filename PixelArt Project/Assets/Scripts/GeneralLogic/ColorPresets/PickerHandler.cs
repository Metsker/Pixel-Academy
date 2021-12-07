using System;
using System.Collections;
using GeneralLogic;
using GeneralLogic.ColorPresets;
using GeneralLogic.Tools.Logic;
using HSVPicker;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EditorMod.ColorPresets
{
    public class PickerHandler : MonoBehaviour
    {
        [SerializeField] private ColorPicker picker;
        private RectTransform _rectTransform;
        private float _startPos = 0;
        private float _endPos = 1;
        private bool _isAnimating;

        private void Awake()
        {
            _rectTransform = picker.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            ColorPreset.EnablePicker += EnablePicker;
            ColorPreset.IsPickerActive += IsPickerActive;
            ColorPreset.DisablePicker += DisablePicker;
            ClickOnPixel.CheckPicker += IsPickerActive;
            ClickOnPixel.DisablePicker += DisablePicker;
            ToolsManager.DisablePicker += DisablePicker;
            BaseTool.DisablePicker += DisablePicker;
            ColorPresetSpawner.EnablePicker += EnablePicker;
            ColorPresetSpawner.DisablePicker += DisablePicker;
        }
        private void OnDisable()
        {
            ColorPreset.EnablePicker -= EnablePicker;
            ColorPreset.IsPickerActive -= IsPickerActive;
            ColorPreset.DisablePicker -= DisablePicker;
            ClickOnPixel.CheckPicker -= IsPickerActive;
            ClickOnPixel.DisablePicker -= DisablePicker;
            ToolsManager.DisablePicker -= DisablePicker;
            BaseTool.DisablePicker -= DisablePicker;
            ColorPresetSpawner.EnablePicker -= EnablePicker;
            ColorPresetSpawner.DisablePicker -= DisablePicker;
        }

        private void EnablePicker(UnityAction<Color> pickerAction, Image image)
        {
            if (!IsPickerActive())
            {
                picker.gameObject.SetActive(true);
                StartCoroutine(PickerAnimation());
            }
            foreach (var ps in ColorPresetSpawner.colorPresets)
            {
                picker.onValueChanged.RemoveListener(ps.pickerAction);
            }
            picker.CurrentColor = image.color;
            picker.onValueChanged.AddListener(pickerAction);
        }

        public void DisablePicker()
        {
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play || !IsPickerActive()) return;
            StartCoroutine(PickerAnimation());
        }
        private bool IsPickerActive()
        {
            return picker.gameObject.activeSelf;
        }

        private IEnumerator PickerAnimation()
        {
            if (_isAnimating) yield break;
            _isAnimating = true;
            
            for (float i = 0; i < 1; i += Time.deltaTime*2)
            {
                _rectTransform.pivot = new Vector2
                    (Mathf.Lerp(_startPos, _endPos, EasingSmoothSquared(i)), _rectTransform.pivot.y);
                yield return null;
            }
            var cash = _startPos;
            _startPos = _endPos;
            _endPos = cash;
            if (_rectTransform.pivot.x < 0.5f)
            {
                picker.gameObject.SetActive(false);
            }
            _isAnimating = false;
        }

        private float EasingSmoothSquared(float x)
        {
            return x < 0.5f ? x * x * 2 : 1 - (1 - x) * (1 - x) * 2;
        }
    }
}