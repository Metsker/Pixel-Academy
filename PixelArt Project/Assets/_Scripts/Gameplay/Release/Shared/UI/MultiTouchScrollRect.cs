using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Shared.UI
{
    public class MultiTouchScrollRect : ScrollRect
    {
#if !UNITY_EDITOR
        private const int MinimumTouchCount = 1;
        private const int MaximumTouchCount = 2;
        private int _pointerId = -100;

        private Vector2 MultiTouchPosition
        {
            get
            {
                Vector2 position = Vector2.zero;
                for (int i = 0; i < Input.touchCount && i < MaximumTouchCount; i++)
                {
                    position += Input.touches[i].position;
                }
                position /= ((Input.touchCount <= MaximumTouchCount) ? Input.touchCount : MaximumTouchCount);
                return position;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (Input.touchCount >= MinimumTouchCount)
            {
                _pointerId = eventData.pointerId;
                eventData.position = MultiTouchPosition;
                base.OnBeginDrag(eventData);
            }
        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount >= MinimumTouchCount)
            {
                eventData.position = MultiTouchPosition;
                if (_pointerId == eventData.pointerId)
                {
                    base.OnDrag(eventData);
                }
            }
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (Input.touchCount >= MinimumTouchCount)
            {
                _pointerId = -100;
                eventData.position = MultiTouchPosition;
                base.OnEndDrag(eventData);
            }
        }
#endif
    }
}
