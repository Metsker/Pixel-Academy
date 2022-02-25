using _Scripts.SharedOverall.Audio;
using UnityEngine;

namespace _Scripts.SharedOverall.Settings
{
    public class EffectsToggler : AudioSetting
    {
        private const string ChannelName = "Effects";
        private void Start()
        {
            image.sprite = IsSoundEnabled() ? sprites[1] : sprites[0];
        }

        private static bool IsSoundEnabled()
        {
            if (AudioManager.AudioMixer.GetFloat(ChannelName, out var value))
            {
                return value > -80;
            }
            return false;
        }
        public override void ToggleClick()
        {
            Toggle(ChannelName);
        }
    }
}