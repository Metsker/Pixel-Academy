using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.Gameplay.Release.Playing.Resulting.UI;
using UnityEngine;

namespace _Scripts.SharedOverall.Audio
{
    public class AudioEffects : BaseAudio
    {
        [SerializeField] private AudioClip notPassedSound;
        [SerializeField] private AudioClip passedSound;
        [SerializeField] private AudioClip perfectSound;
        [SerializeField] private AudioClip starSound;

        private static float _passedSoundLength;
        private static float _perfectSoundLength;

        private void Start()
        {
            _passedSoundLength = passedSound.length;
            _perfectSoundLength = perfectSound.length;
        }

        public enum AudioEffectType
        {
            LevelNotPassed,
            LevelPassed,
            LevelPerfect,
            Stars
        }
        private void OnEnable()
        {
            LevelCompleter.PlaySound += PlaySound;
            RewardCalculator.PlaySound += PlaySound;
        }
        private void OnDisable()
        {
            LevelCompleter.PlaySound -= PlaySound;
            RewardCalculator.PlaySound -= PlaySound;
        }
        private void PlaySound(AudioEffectType audioEffectType)
        {
            switch (audioEffectType)
            {
                case AudioEffectType.LevelNotPassed:
                    audioSource.PlayOneShot(notPassedSound);
                    break;
                case AudioEffectType.LevelPassed:
                    audioSource.PlayOneShot(passedSound);
                    break;
                case AudioEffectType.LevelPerfect:
                    audioSource.PlayOneShot(perfectSound);
                    break;
                case AudioEffectType.Stars:
                    audioSource.PlayOneShot(starSound);
                    break;
            }
        }

        public static float GetPassedSoundLength(RewardCalculator.Result result)
        {
            return result == RewardCalculator.Result.Passed ? _passedSoundLength : _perfectSoundLength;
        }
    }
}