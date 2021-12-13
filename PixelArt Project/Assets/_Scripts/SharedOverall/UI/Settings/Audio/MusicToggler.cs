using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.SharedOverall.Settings
{
    public class MusicToggler : AudioSetting
    {
        private void Start()
        {
            blocker.SetActive(IsMusicEnabled()); //change
        }
        
        public override void ToggleClick()
        {
            Toggle("Music");
        }
        
        public static bool IsMusicEnabled()
        {
            return musicCashValue < 0;
        }
    }
}
