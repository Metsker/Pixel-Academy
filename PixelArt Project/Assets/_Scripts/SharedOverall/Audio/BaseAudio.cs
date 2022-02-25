using UnityEngine;

namespace _Scripts.SharedOverall.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class BaseAudio : MonoBehaviour
    {
        protected AudioSource audioSource;
        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}