using System;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall.UI;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _Scripts.SharedOverall.Ads
{
    public abstract class BaseRewardedAdVideo : MonoBehaviour
    {
        protected const string TestADUnitId = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;
        private WarningUI _warningUI;

        protected void Awake()
        {
            _warningUI = FindObjectOfType<WarningUI>();
        }
        private void Start()
        {
            RequestRewarded();
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
            _rewardedAd = new RewardedAd(GetAdUnitId());

            if (PlayerPrefs.GetInt("NoAds", 0) == 1) return;
            
            _rewardedAd.OnAdLoaded += _warningUI.CheckAdLoaded;
            _rewardedAd.OnUserEarnedReward += GetRewarded;
            _rewardedAd.OnAdClosed += RequestAgain;
            
            var request = new AdRequest.Builder().Build();
            _rewardedAd.LoadAd(request);
        }
        
        public void ShowVideo()
        {
            if (PlayerPrefs.GetInt("NoAds", 0) == 1)
            {
                GetRewarded(this, new Reward());
            }
            else
            {
                _rewardedAd.Show();
            }
            
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