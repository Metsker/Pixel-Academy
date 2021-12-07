using _Scripts.GameplayMod.Resulting;
using _Scripts.GeneralLogic.Menu.Transition;
using _Scripts.GeneralLogic.Saving;
using TMPro;
using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.UI
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
