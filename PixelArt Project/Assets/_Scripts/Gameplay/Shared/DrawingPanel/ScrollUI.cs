using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.DrawingPanel
{
    public class ScrollUI : MonoBehaviour, IScrollHandler
    {
        [SerializeField] private float maxZoom = 8;
        private const float EditorZoomSpeed = 0.1f;
        public static event Action CorrectGrid;

#if !UNITY_EDITOR
     private const float ZoomSpeed = 0.005f; //Test
     private void Update()
        {
            if (Input.touchCount < 2) return;
            var tZero = Input.GetTouch(0);
            var tOne = Input.GetTouch(1);
                
            var tZeroPrevious = tZero.position - tZero.deltaPosition;
            var tOnePrevious = tOne.position - tOne.deltaPosition;

            var oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious); 
            var currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);
                
            var scrollDelta = currentTouchDistance - oldTouchDistance;
            OnScroll(scrollDelta);
        }
        private void OnScroll(float scrollDelta)
        {
            var delta = Vector3.one * (scrollDelta * ZoomSpeed);
            var desiredScale = transform.localScale + delta;
            desiredScale = ClampDesiredScale(desiredScale);
            transform.localScale = desiredScale;
        }   
#endif 

        public void OnScroll(PointerEventData eventData)
        {
#if UNITY_EDITOR
            var delta = Vector3.one * (eventData.scrollDelta.y * EditorZoomSpeed);
            var desiredScale = transform.localScale + delta;
            desiredScale = ClampDesiredScale(desiredScale);
            transform.localScale = desiredScale;
#endif
            CorrectGrid?.Invoke();
            
        }
        private Vector3 ClampDesiredScale(Vector3 desiredScale)
        {
            desiredScale = Vector3.Max(Vector3.one, desiredScale);
            desiredScale = Vector3.Min(Vector3.one * maxZoom, desiredScale);
            return desiredScale;
        }
    }
}
