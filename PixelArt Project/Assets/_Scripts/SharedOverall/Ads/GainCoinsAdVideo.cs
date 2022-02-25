using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Ads
{
    public class GainCoinsAdVideo : BaseAdVideo
    {
        private const int Value = 5;
        private const string ADUnitId = "ca-app-pub-3635129303584563/9467175414";
        
        [SerializeField] private Button gainButton;

        private void Start()
        {
            gainButton.interactable = true;
            RequestRewarded();
        }
        
        protected override void GetRewarded(object sender, Reward reward)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0)+Value);
            PlayerPrefs.Save();
            CoinsUI.UpdateCoinsUI();
            gainButton.interactable = false;
        }
        
        protected override string GetAdUnitId()
        {
            return TestADUnitId; //Change
        }
    }
}