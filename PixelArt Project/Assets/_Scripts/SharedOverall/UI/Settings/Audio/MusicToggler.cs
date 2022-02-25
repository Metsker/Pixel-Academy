using _Scripts.SharedOverall.Audio;

namespace _Scripts.SharedOverall.Settings
{
    public class MusicToggler : AudioSetting
    {
        private const string ChannelName = "Music";
        private void Start()
        {
            image.sprite = IsMusicEnabled() ? sprites[1] : sprites[0];
        }
        
        public override void ToggleClick()
        {
            Toggle("Music");
        }
        
        public static bool IsMusicEnabled()
        {
            if (AudioManager.AudioMixer.GetFloat(ChannelName, out var value))
            {
                return value > -80;
            }
            return false;
        }
    }
}
