using System;
using GameLogic;
using Gameplay;
using HSVPicker;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MapEditor.ColorPresets
{
    public class PickerHandler : MonoBehaviour
    {
        [SerializeField] private ColorPicker picker;
        private void OnEnable()
        {
            ColorPreset.EnablePicker += EnablePicker;
            ColorPreset.DisablePicker += DisablePicker;
            ClickOnPixel.CheckPicker += CheckPicker;
        }
        private void OnDisable()
        {
            ColorPreset.EnablePicker -= EnablePicker;
            ColorPreset.DisablePicker -= DisablePicker;
            ClickOnPixel.CheckPicker -= CheckPicker;
        }

        private void EnablePicker(UnityAction<Color> pickerAction, Image image)
        {
            picker.gameObject.SetActive(true);
            picker.CurrentColor = image.color;
            picker.onValueChanged.AddListener(pickerAction);
        }

        private bool CheckPicker()
        {
            if (!picker.gameObject.activeSelf) return false;
            foreach (var ps in FindObjectsOfType<ColorPreset>())
            {
                picker.onValueChanged.RemoveListener(ps.pickerAction);
            }
            picker.gameObject.SetActive(false);
            return true;
        }
        
        public void DisablePicker()
        {
            if (!picker.gameObject.activeSelf) return;
            foreach (var ps in FindObjectsOfType<ColorPreset>())
            {
                picker.onValueChanged.RemoveListener(ps.pickerAction);
            }
            picker.gameObject.SetActive(false);
        }
    }
}