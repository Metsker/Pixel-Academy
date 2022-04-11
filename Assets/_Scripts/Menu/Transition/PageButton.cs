using System;
using System.Linq;
using _Scripts.Menu.Transition;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.Menu.Transition.PageManager;

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
        private static Pages? _scheduledPage;
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
            if (CurrentPage == page) return;
            Pointer(Duration);
        }

        public void Pointer(float duration)
        {
            if (IsAnimating)
            {
                _scheduledPage = page;
                return;
            }
            IsAnimating = true;
            
            SetDirection();
            foreach (var arrow in _pageManager.arrows.FindAll(a => a.GetPageType() == CurrentPage))
            {
                arrow.FadeOut();
            }
#if UNITY_ANDROID
            PreviousPage = CurrentPage;
#endif
            CurrentPage = page;
            PageManager.StageController.SetParticlesLayer(0);
            objToMove.transform.SetSiblingIndex(_sublimingIndex);
            objToMove.SetActive(true);
            background.SetActive(true);
            
            Select();
            foreach (var m in _pageManager.PageButtons.Where(m => m != this))
            {
                m.Deselect();
            }

            _objRectTransform.pivot = Vector2.one/2;
            objToMove.transform.DOMoveX(0, duration).SetEase(Ease)
                .OnComplete(() =>
            {
                background.SetActive(false);
                foreach (var m in _pageManager.PageButtons.Where(m => m != this))
                {
                    m.objToMove.SetActive(false);
                }
                foreach (var arrow in _pageManager.arrows.FindAll(a => a.GetPageType() == page))
                {
                    arrow.TryFadeIn();
                }
                PageManager.StageController.SetParticlesLayer(1);
                
                if (page == Pages.Stats) ClearNotification?.Invoke();
                
                IsAnimating = false;
                
                if (_scheduledPage == null) return;
                _pageManager.PointToPage(_scheduledPage.Value, duration);
                _scheduledPage = null;
            });
        }
        
        private void SetDirection()
        {
            var m = (int)page - (int)CurrentPage;
            var newDir = m > 0 ? Direction.Right : Direction.Left;

            ((RectTransform)_pageManager.PageButtons[(int)page].objToMove.transform).pivot = _pageManager.GetPivot(newDir);
            _pageManager.PageButtons[(int)page].objToMove.transform.position = _pageManager.GetPos(newDir);
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
