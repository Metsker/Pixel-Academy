using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GeneralLogic.Ads
{
    public class AdBanner : MonoBehaviour
    {
        private const string ADUnitId = "ca-app-pub-3635129303584563/4382369699";
        private const string TestADUnitId = "ca-app-pub-3940256099942544/6300978111";
        private BannerView _bannerView;

        public void Start()
        {
            MobileAds.Initialize(_ => { });
            RequestBanner();
        }

        private void RequestBanner()
        {
            _bannerView = new BannerView(TestADUnitId, AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(Screen.width), 
                SceneManager.GetActiveScene().buildIndex == 0 ? AdPosition.Top : AdPosition.Bottom);
            var request = new AdRequest.Builder().Build();
            _bannerView.LoadAd(request);
        }
    }
}
