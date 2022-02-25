using _Scripts.Gameplay.Playing.Hints;
using GoogleMobileAds.Api;

namespace _Scripts.SharedOverall.Ads
{
    public class HintAdVideo : BaseAdVideo
    {
        private const string ADUnitId = "ca-app-pub-3635129303584563/9467175414";
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

        private void Start()
        {
            RequestRewarded();
        }

        protected override void GetRewarded(object sender, Reward reward)
        {
            if (HintReference == null) return;
            HintReference.TakeHint();
        }
        protected override string GetAdUnitId()
        {
            return TestADUnitId; //Change
        }
    }
}