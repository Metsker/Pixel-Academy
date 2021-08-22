using System;
using DG.Tweening;
using UnityEngine;

namespace GameplayMod.UI
{
    public class StageAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject levelsParent;
        [SerializeField] private GameObject endPoint;
        [Space]
        [SerializeField] private GameObject[] toHide;
        [SerializeField] private GameObject[] particleLayers;
        [SerializeField] private ParticlesSpawner particlesSpawner;
        [Space]
        [SerializeField] private Ease ease;

        private const float Duration = 1.2f;
        private static Vector2 _startPos;
        private static bool _isAnimating;

        private void Start()
        {
            _isAnimating = false;
            _startPos = levelsParent.transform.position;
            levelsParent.transform.position = endPoint.transform.position;
        }

        public void ShowLevels()
        {
            if(_isAnimating) return;
            _isAnimating = true;
            levelsParent.SetActive(true);
            particlesSpawner.transform.SetParent(particleLayers[1].transform, false);
            levelsParent.transform.DOMove(_startPos, Duration).SetEase(ease).OnComplete(() =>
            {
                foreach (var g in toHide)
                {
                    g.SetActive(false);
                }
                particlesSpawner.transform.SetParent(particleLayers[0].transform, false);
                _isAnimating = false;
            });
        }

        public void HideLevels()
        {
            if(_isAnimating) return;
            _isAnimating = true;
            foreach (var g in toHide)
            {
                g.SetActive(true);
            }
            particlesSpawner.transform.SetParent(particleLayers[1].transform, false);
            levelsParent.transform.DOMove(endPoint.transform.position, Duration).SetEase(ease)
                .OnComplete(() =>
                {
                    levelsParent.SetActive(false);
                    particlesSpawner.transform.SetParent(particleLayers[0].transform, false);
                    _isAnimating = false;
                });
        }
    }
}
