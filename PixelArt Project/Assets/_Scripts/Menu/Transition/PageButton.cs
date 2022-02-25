using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets._Scripts.Menu.Transition.PageManager;

namespace Assets._Scripts.Menu.Transition
{
    public class PageButton : MonoBehaviour, IPointerUpHandler
    {
        public GameObject objToMove;
        [SerializeField] private GameObject background;
        public Pages page;
        private static PageManager _pageManager;

        private const Ease Ease = DG.Tweening.Ease.InOutSine;
        private Image _selectImage;
        private RectTransform _objRectTransform;

        private static int _sublimingIndex;
        public static bool IsAnimating { get; private set; }
        public static event Action ClearNotification;

        private void Awake()
        {
            _selectImage = GetComponent<Image>();
            _objRectTransform = objToMove.GetComponent<RectTransform>();
            if (_pageManager == null)
            {
                _pageManager = FindObjectOfType<PageManager>();
            }
            if (_sublimingIndex == 0)
            {
                _sublimingIndex = objToMove.transform.parent.childCount - 1;
            }
            if (!IsAnimating) return;
            IsAnimating = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CashPage = CurrentPage;
            if (CurrentPage == page) return;
            Pointer(Duration);
        }

        public void Pointer(float duration)
        {
            if(IsAnimating) return;
            
            IsAnimating = true;

            SetDirection();
            CurrentPage = page;
            PageManager.StageController.SetParticlesLayer(0);
            objToMove.transform.SetSiblingIndex(_sublimingIndex);
            objToMove.SetActive(true);
            background.SetActive(true);
            
            Select();
            foreach (var m in pages.Where(m => m != this))
            {
                m.Deselect();
            }
            foreach (var arrow in _pageManager.arrows)
            {
                arrow.FadeOut();
            }

            _objRectTransform.pivot = Vector2.one/2;
            objToMove.transform.DOMoveX(0, duration).SetEase(Ease)
                .OnComplete(() =>
            {
                background.SetActive(false);
                foreach (var m in pages.Where(m => m != this))
                {
                    m.objToMove.SetActive(false);
                }
                foreach (var arrow in _pageManager.arrows)
                {
                    arrow.FadeIn();
                }
                PageManager.StageController.SetParticlesLayer(1);
                IsAnimating = false;
                if (CurrentPage != Pages.Stats) return;
                ClearNotification?.Invoke();
            });
        }
        
        private void SetDirection()
        {
            var m = (int)page - (int)CurrentPage;
            var newDir = m > 0 ? Direction.Right : Direction.Left;

            ((RectTransform)pages[(int)page].objToMove.transform).pivot = _pageManager.GetPivot(newDir);
            pages[(int)page].objToMove.transform.position = _pageManager.GetPos(newDir);

        }

        private void Select()
        {
            _selectImage.DOFade(1,0);
        }

        private void Deselect()
        {
            _selectImage.DOFade(0,0);
        }
    }
}
