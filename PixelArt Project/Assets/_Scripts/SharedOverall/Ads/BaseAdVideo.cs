using System;
using _Scripts.GameplayMod.UI;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _Scripts.GeneralLogic.Ads
{
    public abstract class BaseAdVideo : MonoBehaviour
    {
        protected const string TestADUnitId = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;
        private WarningUI _warningUI;

        private void Awake()
        {
            _warningUI = FindObjectOfType<WarningUI>();
        }
        protected void OnDisable()
        {
            if (_rewardedAd == null) return;
            _rewardedAd.OnAdLoaded -= _warningUI.CheckAdLoaded;
            _rewardedAd.OnUserEarnedReward -= GetRewarded;
            _rewardedAd.OnAdClosed -= RequestAgain;
        }
        public void RequestRewarded()
        {
            var request = new AdRequest.Builder().Build();
            _rewardedAd = new RewardedAd(GetAdUnitId());

            _rewardedAd.OnAdLoaded += _warningUI.CheckAdLoaded;
            _rewardedAd.OnUserEarnedReward += GetRewarded;
            _rewardedAd.OnAdClosed += RequestAgain;
            
            _rewardedAd.LoadAd(request);
        }
        
        public void ShowVideo()
        {
            _rewardedAd.Show();
        }
        private void RequestAgain(object sender, EventArgs eventArgs)
        {
            RequestRewarded();
        }
        public bool IsAdLoaded()
        {
            return _rewardedAd.IsLoaded();
        }
        protected abstract string GetAdUnitId();
        protected abstract void GetRewarded(object sender, Reward reward);
    }
}