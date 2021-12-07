using _Scripts.Menu.Data;
using GoogleMobileAds.Api;

namespace _Scripts.SharedOverall.Ads
{
    public class LevelAdVideo : BaseAdVideo
    {
        private const string ADUnitId = "ca-app-pub-3635129303584563/9467175414";
        public static LevelData LevelDataReference { get; set; }
        
        private void Start()
        {
            RequestRewarded();
        }
        
        protected override void GetRewarded(object sender, Reward reward)
        {
            if (LevelDataReference == null) return;
            LevelDataReference.Unlock();
        }

        protected override string GetAdUnitId()
        {
            return TestADUnitId; //Change
        }
    }
}