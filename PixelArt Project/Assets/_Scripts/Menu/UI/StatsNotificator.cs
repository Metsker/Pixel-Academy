using _Scripts.Menu.Transition;
using TMPro;
using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class StatsNotificator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI notificationCount;

        private void Start()
        {
            SetNotification();
        }
        private void OnEnable()
        {
            PageButton.ClearNotification += ResetNotification;
        }
        private void OnDisable()
        {
            PageButton.ClearNotification -= ResetNotification;
        }
        
        private void SetNotification()
        {
            if (PlayerPrefs.GetInt("HasNewAchievement", 0) != 1) return;
            notificationCount.transform.parent.gameObject.SetActive(true);
        }
        
        private void ResetNotification()
        {
            if (!notificationCount.transform.parent.gameObject.activeSelf) return;
            PlayerPrefs.SetInt("HasNewAchievement", 0);
            notificationCount.transform.parent.gameObject.SetActive(false);
        }
    }
}
