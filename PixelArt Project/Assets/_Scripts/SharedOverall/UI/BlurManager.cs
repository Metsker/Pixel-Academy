using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Playing.Resulting.UI;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.UI;
using _Scripts.SharedOverall.UI.Settings;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Shared.UI
{
    public class BlurManager : MonoBehaviour
    {
        [SerializeField] private ClipManager clipManager;
        private static GameObject _blur;

        private void Awake()
        {
            _blur = transform.GetChild(0).gameObject;
        }
        private void OnEnable()
        {
            LevelCompleter.SwitchBlur += SwitchBlur;
            WarningUI.SwitchBlur += SwitchBlur;
            SettingsUI.SwitchBlur += SwitchBlur;
        }
        private void OnDisable()
        {
            LevelCompleter.SwitchBlur -= SwitchBlur;
            WarningUI.SwitchBlur -= SwitchBlur;
            SettingsUI.SwitchBlur -= SwitchBlur;
        }
        
        private void SwitchBlur(bool state)
        {
            switch (GameStateManager.CurrentGameState)
            {
                case GameStateManager.GameState.Animating when clipManager != null:
                    clipManager.SetAnimatorState(state);
                    break;
                case GameStateManager.GameState.Correcting:
                    ResultCorrector.SetCorrectionState(state);
                    break;
            }
            _blur.SetActive(state);
        }

        public static bool IsBlured()
        {
            return _blur.activeSelf;
        }
    }
}
