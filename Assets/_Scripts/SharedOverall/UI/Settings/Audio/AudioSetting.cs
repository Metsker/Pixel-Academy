using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.UI.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.SharedOverall.Settings
{
    public abstract class AudioSetting : Setting
    {
        
        protected void Toggle(string chanelName)
        {
            if (!AudioManager.AudioMixer.GetFloat(chanelName, out var value)) return;
            switch (value)
            {
                case 0:
                    AudioManager.AudioMixer.SetFloat(chanelName, -80);
                    image.sprite = sprites[0];
                    break;
                default:
                    AudioManager.AudioMixer.SetFloat(chanelName, 0);
                    image.sprite = sprites[1];
                    break;
            }
        }
    }
}