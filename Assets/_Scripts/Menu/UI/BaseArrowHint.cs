using System.Collections;
using _Scripts.Menu.Transition;
using _Scripts.SharedOverall.Utility;
using Assets._Scripts.Menu.Transition;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu.UI
{
    public abstract class BaseArrowHint : MonoBehaviour
    {
        [SerializeField] private RectTransform anchor;
        [SerializeField] private Axis axis;
        [SerializeField] private PageManager.Pages page;
        
        private Image _image;
        private Sequence _sequence;
        private readonly Ease ease = Ease.InOutSine;
        private const float AnimDuration = 0.75f;
        private const float FadeDuration = 0.1f;

        private enum Axis
        {
            X,Y
        }
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            if (page != PageManager.Pages.Main)
            {
                yield return new WaitUntil(() => !PageButton.IsAnimating);
            }
            if (!TryFadeIn()) yield break;
            switch (axis)
            {
                case Axis.X:
                    _sequence = DOTween.Sequence().
                        Append(transform.DOMoveX(anchor.position.x, AnimDuration).
                            SetEase(ease))
                        .SetLoops(-1, LoopType.Yoyo);
                    break;
                case Axis.Y:
                    _sequence = DOTween.Sequence().
                        Append(transform.DOMoveY(anchor.position.y, AnimDuration).
                            SetEase(ease))
                        .SetLoops(-1, LoopType.Yoyo);
                    break;
            }
        }
        public bool TryFadeIn()
        {
            if (IsShown()) return false;
            _image.DOFade(1, FadeDuration);
            return true;
        }
        public void FadeOut()
        {
            if (IsShown()) return;
            _image.DOFade(0, FadeDuration);
        }
        public void OnBeginDrag()
        {
            _sequence.Kill();
            FadeOut();
            SetShown();
        }
        protected abstract bool IsShown();
        protected abstract void SetShown();

        public PageManager.Pages GetPageType()
        {
            return page;
        }
    }
}
