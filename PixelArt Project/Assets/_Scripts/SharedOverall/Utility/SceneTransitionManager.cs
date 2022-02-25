using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Utility
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private GameObject fadeCamera;
        
        [SerializeField] private Image fadeImage;
        [SerializeField] private LoadingAnimation loadingAnimation;
        
        private static GameObject _fadeCamera;
        private static Image _fadeImage;
        private static LoadingAnimation _loadingAnimation;
        private const float FadeDuration = 0.5f;
        private const float BaseLoadingDuration = 0.1f;

        public enum Scenes
        {
            Additive,
            Menu,
            Play
        }
        
        private void Awake()
        {
            _fadeCamera = fadeCamera;
            _fadeImage = fadeImage;
            _loadingAnimation = loadingAnimation;
        }
        private void OnDisable()
        {
            DOTween.KillAll();
            
        }
        private IEnumerator Start()
        {
            fadeCamera.SetActive(true);
            loadingAnimation.transform.rotation = LoadingAnimation.CashRotation;
            loadingAnimation.Animate();

            var t1 = Time.time;
            yield return LocalizationSettings.InitializationOperation;
            yield return new WaitUntil(LanguageManager.IsLanguageInitialized);
            var t2 = Time.time;
            var r = t2 - t1;
            if (r < BaseLoadingDuration)
            {
                yield return new WaitForSeconds(r);
            }
            loadingAnimation.ToggleState(false);
            _fadeImage.DOFade(0, FadeDuration).OnComplete(() =>
                fadeCamera.SetActive(false));
        }
        
        public static void OpenScene(Scenes scene)
        {
            _fadeCamera.SetActive(true);
            _fadeImage.DOFade(1, FadeDuration).OnComplete(() =>
            {
                _loadingAnimation.ToggleState(true);
                _loadingAnimation.Animate();
                SceneManager.LoadSceneAsync((int)scene);
            });
        }
        public static bool IsLoaded()
        {
            return _fadeImage.color.a == 0;
        }
    }
}