using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private GameObject _blocker;
    private static float _musicCashValue;

    private void Awake()
    {
        _blocker = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        _blocker.SetActive(IsMusicEnabled());
    }

    public void ToggleMusic()
    {
        if (!audioMixer.GetFloat("Music", out _musicCashValue)) return;
        switch (_musicCashValue)
        {
            case 0:
                audioMixer.SetFloat("Music", -80);
                _blocker.SetActive(true);
                _musicCashValue = -80;
                break;
            default:
                audioMixer.SetFloat("Music", 0);
                _blocker.SetActive(false);
                _musicCashValue = 0;
                break;
        }
    }

    public static bool IsMusicEnabled()
    {
        return _musicCashValue < 0;
    }
}
