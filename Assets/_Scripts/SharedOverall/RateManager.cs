using System;
using System.Collections;
using _Scripts.Gameplay.Playing.Resulting;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
using UnityEngine;

namespace _Scripts.SharedOverall
{
    public class RateManager : MonoBehaviour
    {
#if UNITY_ANDROID
        private int _launchCount;
        private PlayReviewInfo _playReviewInfo;
        private ReviewManager _reviewManager;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _launchCount = PlayerPrefs.GetInt("TimesLaunched", 0);
            _launchCount++;
            PlayerPrefs.SetInt("TimesLaunched", _launchCount);

            if (_launchCount == 2 || _launchCount == 4) StartRequest();
        }

        private void OnEnable()
        {
            RewardCalculator.RateRequest += StartRequest;
        }

        private void OnDisable()
        {
            RewardCalculator.RateRequest -= StartRequest;
        }
        
        private void StartRequest()
        {
            StartCoroutine(RateRequest());
        }
        
        private IEnumerator RateRequest()
        {
            _reviewManager = new ReviewManager();
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError) yield break;

            _playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError) yield break;
        }
#endif
    }
}