using System;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.SharedOverall.DrawingPanel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Animating
{
    public class BorderManager : MonoBehaviour
    {
        [SerializeField] private Image pauseImage;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite unpauseSprite;

        public RectTransform panelRect;
        [SerializeField] private RectTransform fGridTransform;
        private FlexibleGridLayout _flexibleGridLayout;
        
        
        
        private const float AnimationDuration = 0.3f;
        private const float PauseOpacity = 0.7f;
        
        private bool _isAnimating;

        private void Awake()
        {
            _flexibleGridLayout = fGridTransform.GetComponent<FlexibleGridLayout>();
        }

        private void OnEnable()
        {
            ClipManager.SwitchPause += SwitchPauseAnimation;
            ResultCorrector.SwitchPause += SwitchPauseAnimation;
            ScrollUI.CorrectGrid += CorrectGrid;
            _flexibleGridLayout.SetUpGrid += SetUpGrid;
        }

        private void OnDisable()
        {
            ClipManager.SwitchPause -= SwitchPauseAnimation;
            ResultCorrector.SwitchPause -= SwitchPauseAnimation;
            ScrollUI.CorrectGrid -= CorrectGrid;
            _flexibleGridLayout.SetUpGrid -= SetUpGrid;
        }
        

        public void ToggleState(bool state)
        {
            if(panelRect.gameObject.activeSelf == state) return;
            panelRect.gameObject.SetActive(state);
        }

        private void SetUpGrid()
        {
            ToggleState(true);
            panelRect.sizeDelta = fGridTransform.sizeDelta;
        }
        
        public void CorrectGrid()
        {
            panelRect.localPosition = fGridTransform.localPosition;
            panelRect.localScale = fGridTransform.localScale;
        }
        private void SwitchPauseAnimation(bool animatorActiveState)
        {
            pauseImage.sprite = animatorActiveState ? pauseSprite : unpauseSprite;
            pauseImage.DOFade(PauseOpacity, AnimationDuration);
            DOTween.Sequence().AppendCallback(
                () =>
                {
                    if (_isAnimating) return;
                    _isAnimating = true;
                    pauseImage.gameObject.transform.DOPunchScale(Vector3.one * 0.25f, AnimationDuration, 1)
                        .OnComplete(() => _isAnimating = false);
                }).AppendInterval(0.6f).Append(pauseImage.DOFade(0, AnimationDuration));
        }
    }
}