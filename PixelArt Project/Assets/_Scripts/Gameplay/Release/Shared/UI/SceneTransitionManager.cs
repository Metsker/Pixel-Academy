using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace _Scripts.GameplayMod.Animating
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private LoadingAnimation loadingAnimation;
        
        private static Image _fadeImage;
        private static LoadingAnimation _loadingAnimation;
        private const float FadeDuration = 0.5f;

        private void Awake()
        {
            _fadeImage = fadeImage;
            _loadingAnimation = loadingAnimation;
        }
        private void OnDisable()
        {
            DOTween.KillAll();
        }
        private IEnumerator Start()
        {
            _fadeImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _fadeImage.DOFade(0, FadeDuration);
        }
        
        public static void OpenScene(int index)
        {
            _fadeImage.raycastTarget = true;
            _fadeImage.DOFade(1, FadeDuration).OnComplete(() =>
            {
                _loadingAnimation.ToggleState(true);
                _loadingAnimation.Animate();
                SceneManager.LoadSceneAsync(index);
            });
        }
        public static bool IsLoaded()
        {
            return _fadeImage.color.a == 0;
        }
    }
}