using System.Collections;
using _Scripts.SharedOverall.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu.UI
{
    public abstract class BaseArrowHint : MonoBehaviour
    {
        [SerializeField] private RectTransform anchor;
        
        private Image _image;
        private Sequence _sequence;
        private readonly Ease ease = Ease.InOutSine;
        private const float AnimDuration = 0.75f;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        protected IEnumerator Start()
        {
            if (IsShown()) yield break;
            
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            _image.DOFade(1, 0.1f);
            _sequence = DOTween.Sequence().
                Append(transform.DOMoveX(anchor.position.x, AnimDuration).
                    SetEase(ease))
                .SetLoops(-1, LoopType.Yoyo);
        }
        public void OnBeginDrag()
        {
            _sequence.Kill();
            FadeOut();
            SetShown();
        }
        public void FadeIn()
        {
            _image.DOFade(1, 0.1f);
        }
        public void FadeOut()
        {
            _image.DOFade(0, 0.1f);
        }
        protected abstract bool IsShown();
        protected abstract void SetShown();
    }
}
