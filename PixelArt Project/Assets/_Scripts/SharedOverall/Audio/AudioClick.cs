using System;
using _Scripts.GameplayMod.Animating;
using _Scripts.GameplayMod.Resulting;
using _Scripts.GeneralLogic.Menu.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.GeneralLogic.Audio
{
    public class AudioClick : BaseAudio
    {
        [SerializeField] private AudioClip uiSound;
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip toolSound;

        public enum AudioClickType
        {
            UI,
            Tool,
            Click
        }

        private void OnEnable()
        {
            AudioClickEvent.PlaySound += PlaySound;
            ResultCalculator.PlaySound += PlaySound;
            AnimationEventHandler.PlaySound += PlaySound;
            SizeStep.PlaySound += PlaySound;
        }
        private void OnDisable()
        {
            AudioClickEvent.PlaySound -= PlaySound;
            ResultCalculator.PlaySound -= PlaySound;
            AnimationEventHandler.PlaySound -= PlaySound;
            SizeStep.PlaySound -= PlaySound;
        }

        private void PlaySound(AudioClickType audioClickType)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            switch (audioClickType)
            {
                case AudioClickType.UI:
                    audioSource.PlayOneShot(uiSound);
                    break;
                case AudioClickType.Tool:
                    audioSource.PlayOneShot(toolSound);
                    break;
                case AudioClickType.Click:
                    audioSource.PlayOneShot(clickSound);
                    break;
            }
        }
    }
}