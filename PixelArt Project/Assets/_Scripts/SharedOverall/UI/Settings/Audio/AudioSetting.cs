using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.SharedOverall.Settings
{
    public abstract class AudioSetting : Setting
    {
        [SerializeField] protected AudioMixer audioMixer;
        protected GameObject blocker; //sprite, image
        protected static float musicCashValue;
        
        private void Awake()
        {
            blocker = transform.GetChild(0).gameObject; //change
        }

        protected void Toggle(string chanelName)
        {
            if (!audioMixer.GetFloat(chanelName, out musicCashValue)) return;
            switch (musicCashValue)
            {
                case 0:
                    audioMixer.SetFloat(chanelName, -80);
                    blocker.SetActive(true); //sprite change
                    musicCashValue = -80;
                    break;
                default:
                    audioMixer.SetFloat(chanelName, 0);
                    blocker.SetActive(false); //sprite change
                    musicCashValue = 0;
                    break;
            }
        }
    }
}