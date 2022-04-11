using System;
using System.Collections;
using _Scripts.Menu;
using _Scripts.SharedOverall.Utility;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall.Ads
{
    public class AdBanner : MonoBehaviour
    {
        [SerializeField] private AdPosition adPosition;
        
        private const string ADUnitId = "ca-app-pub-3635129303584563/4382369699";
        private const string TestADUnitId = "ca-app-pub-3940256099942544/6300978111";
        private BannerView _bannerView;

        private void OnEnable()
        {
            DonationManager.DestroyBanner += DestroyBanner;
        }
        private void OnDisable()
        {
            DestroyBanner();
            DonationManager.DestroyBanner -= DestroyBanner;
        }

        public IEnumerator Start()
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Record) yield break;
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            
            if (PlayerPrefs.GetInt("NoAds", 0) == 1) yield break;
            MobileAds.Initialize(_ => { });
            RequestBanner();
        }
        private void RequestBanner()
        {
            var id = Application.isEditor ? TestADUnitId : ADUnitId;
            _bannerView = new BannerView(id, AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(Screen.width), adPosition);
            var request = new AdRequest.Builder().Build();
            _bannerView.LoadAd(request);
        }
        private void DestroyBanner()
        {
            _bannerView?.Destroy();
        }
    }
}
