using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Ads
{
    public class GainCoinsRewardedAdVideo : BaseRewardedAdVideo
    {
        private const int Value = 5;
        private const string ADUnitId = "ca-app-pub-3635129303584563/4622317850";
        [SerializeField] private Button gainButton;

        protected new void Awake()
        {
            base.Awake();
            SwitchGain(PlayerPrefs.GetInt("CanGain", 1) == 1);
        }

        protected override void GetRewarded(object sender, Reward reward)
        {
            PlayerPrefs.SetInt("CanGain", 0);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + Value);
            PlayerPrefs.Save();
            CoinsUI.UpdateCoinsUI();
            SwitchGain(false);
        }

        private void SwitchGain(bool state)
        {
            //gainButton.interactable = state;
        }

        protected override string GetAdUnitId()
        {
            return Application.isEditor ? TestADUnitId : ADUnitId;
        }
    }
}