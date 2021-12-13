using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Playing.Animating
{
    public class DirectionHelper : MonoBehaviour
    {
        [SerializeField] private Image arrow;
        private BoxCollider2D _viewPort;

        private void Start()
        {
            _viewPort = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            ClickOnPixel.SetHelpDirection += SetHelpDirection;
            ClipManager.SetHelpDirection += SetHelpDirection;
            ResultCalculator.SetHelpDirection += SetHelpDirection;
        }

        private void OnDisable()
        {
            ClickOnPixel.SetHelpDirection -= SetHelpDirection;
            ClipManager.SetHelpDirection -= SetHelpDirection;
            ResultCalculator.SetHelpDirection -= SetHelpDirection;
        }

        private void SetHelpDirection(Vector3 targetPos, bool state)
        {
            if (state == false)
            {
                arrow.gameObject.SetActive(false);
                return;
            }
            arrow.transform.localPosition = Vector3.zero;
            arrow.gameObject.SetActive(true);
            
            var dir = targetPos.normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }
            var rect = (RectTransform)arrow.transform;
            rect.eulerAngles = new Vector3(0,0,angle);

            while (_viewPort.bounds.Contains(arrow.transform.position))
            {
                var localPosition = arrow.transform.localPosition;
                localPosition = new Vector3(localPosition.x + dir.x, localPosition.y + dir.y, 0);
                arrow.transform.localPosition = localPosition;
            }
            arrow.transform.localPosition *= 0.75f;
        }
    }
}
