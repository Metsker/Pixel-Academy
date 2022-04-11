using _Scripts.Gameplay.Playing.Hints;
using GoogleMobileAds.Api;
using UnityEngine.Device;

namespace _Scripts.SharedOverall.Ads
{
    public class HintRewardedAdVideo : BaseRewardedAdVideo
    {
        private const string ADUnitId = "ca-app-pub-3635129303584563/3117664494";
        public static BaseHint HintReference { get; set; }

        private void OnEnable()
        {
            BaseHint.ShowAd += ShowVideo;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            BaseHint.ShowAd -= ShowVideo;
        }

        protected override void GetRewarded(object sender, Reward reward)
        {
            if (HintReference == null) return;
            HintReference.TakeHint();
        }
        protected override string GetAdUnitId()
        {
            return Application.isEditor ? TestADUnitId : ADUnitId;
        }
    }
}