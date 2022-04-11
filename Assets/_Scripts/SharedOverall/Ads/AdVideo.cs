using System;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Playing.Resulting.UI;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _Scripts.SharedOverall.Ads
{
    public class AdVideo : MonoBehaviour
    {
        private const string ADUnitId = "ca-app-pub-3635129303584563/7986847794";
        private InterstitialAd interstitial;

        private void Start()
        {
            if (PlayerPrefs.GetInt("NoAds", 0) == 0)
            {
                RequestInterstitial();
            }
        }

        private void OnEnable()
        {
            LevelCompleter.ShowAd += ShowAd;
        }
        private void OnDisable()
        {
            LevelCompleter.ShowAd -= ShowAd;
            interstitial.OnAdClosed -= HandleOnAdClosed;
        }

        private void RequestInterstitial()
        {
            interstitial = new InterstitialAd(ADUnitId);
            interstitial.OnAdClosed += HandleOnAdClosed;
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
        }
        
        private void ShowAd()
        {
            if (PlayerPrefs.GetInt("NoAds", 0) == 0 &&
                interstitial.IsLoaded()) 
            {
                interstitial.Show();
                RequestInterstitial();
            }
        }
        
        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            interstitial.Destroy();
        }
    }
}