﻿using UnityEngine;

namespace _Scripts.GeneralLogic.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class BaseAudio : MonoBehaviour
    {
        protected AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}