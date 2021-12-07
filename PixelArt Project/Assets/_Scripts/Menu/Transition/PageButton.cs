using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.Menu.Transition.PageManager;

namespace _Scripts.Menu.Transition
{
    public class PageButton : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private GameObject objToMove;
        [SerializeField] private GameObject background;
        [SerializeField] private Transform startPos;
        [SerializeField] private PageManager pageManager;
        [SerializeField] private Pages page;
        [SerializeField] private Ease ease;

        private Image _selectImage;
        private RectTransform _objRectTransform;
        private Vector2 _startPivot;
        
        private static int _sublimingIndex;
        private static bool _isAnimating;
        public static event Action ClearNotification;
        
        private void Awake()
        {
            _selectImage = GetComponent<Image>();
            _objRectTransform = objToMove.GetComponent<RectTransform>();
            
            objToMove.transform.position = startPos.transform.position;
            _startPivot = _objRectTransform.pivot;
            if(_sublimingIndex == 0) _sublimingIndex = objToMove.transform.parent.childCount - 1;
            if (!_isAnimating) return;
            _isAnimating = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            cashPage = currentPage;
            if (currentPage == page) return;
            Pointer(Duration);
        }

        public void Pointer(float duration)
        {
            if(_isAnimating) return;
            
            _isAnimating = true;
            currentPage = page;
            objToMove.transform.SetSiblingIndex(_sublimingIndex);
            objToMove.SetActive(true);
            background.SetActive(true);
            
            Select();
            foreach (var m in pageManager.pages.Where(m => m != this))
            {
                m.Deselect();
            }
            ClearNotification?.Invoke();
            
            _objRectTransform.pivot = Vector2.one/2;
            objToMove.transform.DOMove(new Vector3(0,0,90), duration).SetEase(ease)
                .OnComplete(() =>
            {
                background.SetActive(false);
                foreach (var m in pageManager.pages.Where(m => m != this))
                {
                    m.objToMove.SetActive(false);
                    m.objToMove.transform.position = m.startPos.transform.position;
                }
                switch (page)
                { 
                    case Pages.Editor:
                        SetMainPos(Pages.Stats);
                        break;
                    case Pages.Stats:
                        SetMainPos(Pages.Editor);
                        break; 
                }
                _objRectTransform.pivot = _startPivot;
                _isAnimating = false;
            });
        }
        
        private void SetMainPos(Pages refPage)
        {
            ((RectTransform)pageManager.pages[(int)Pages.Main].objToMove.transform).pivot = ((RectTransform)pageManager.pages[(int)refPage].objToMove.transform).pivot;
            pageManager.pages[(int)Pages.Main].objToMove.transform.position = pageManager.pages[(int)refPage].startPos.transform.position;
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
