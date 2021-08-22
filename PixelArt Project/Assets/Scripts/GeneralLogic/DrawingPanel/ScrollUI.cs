using UnityEngine;
using UnityEngine.EventSystems;

namespace GeneralLogic.DrawingPanel
{
    public class ScrollUI : MonoBehaviour, IScrollHandler
    {
        [SerializeField] private float zoomSpeed = 0.1f;
        [SerializeField] private float maxZoom = 10f;
        

        public void OnScroll(PointerEventData eventData)
        {
            var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
            var desiredScale = transform.localScale + delta;

            desiredScale = ClampDesiredScale(desiredScale);
            transform.localScale = desiredScale;
        }

        private Vector3 ClampDesiredScale(Vector3 desiredScale)
        {
            desiredScale = Vector3.Max(Vector3.one, desiredScale);
            desiredScale = Vector3.Min(Vector3.one * maxZoom, desiredScale);
            return desiredScale;
        }
    }
}
