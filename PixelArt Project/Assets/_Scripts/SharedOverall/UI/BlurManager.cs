using _Scripts.Gameplay.Release.Playing.Resulting.UI;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Shared.UI
{
    public class BlurManager : MonoBehaviour
    {
        private GameObject _blur;

        private void Awake()
        {
            _blur = transform.GetChild(0).gameObject;
        }
        private void OnEnable()
        {
            LevelCompleter.SwitchBlur += SwitchBlur;
            WarningUI.SwitchBlur += SwitchBlur;
        }
        private void OnDisable()
        {
            LevelCompleter.SwitchBlur -= SwitchBlur;
            WarningUI.SwitchBlur -= SwitchBlur;
        }
        private void SwitchBlur()
        {
            _blur.SetActive(!_blur.activeSelf);
        }
    }
}
