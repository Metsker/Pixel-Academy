using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Scripts.Menu
{
    public class DonationManager : MonoBehaviour
    {
        [SerializeField] private GameObject nonConsumable;
        [SerializeField] private GameObject restoreButton;
        
        private const int CoinsDonationValue = 30;
        public static event Action DestroyBanner;

        private void Awake()
        {
#if !UNITY_IOS
            restoreButton.SetActive(false);
#endif
            if (PlayerPrefs.GetInt("NoAds") != 1) return;
            nonConsumable.SetActive(false);
        }
        
        public void OnNoAdsPurchase(Product _)
        {
            PlayerPrefs.SetInt("NoAds", 1);
            PlayerPrefs.Save();
            DestroyBanner?.Invoke();
        }
        public void OnCoinsPurchase(Product _)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + CoinsDonationValue);
            PlayerPrefs.Save();
            CoinsUI.UpdateCoinsUI();
        }
    }
}
