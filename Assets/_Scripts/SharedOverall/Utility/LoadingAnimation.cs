using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.SharedOverall.Utility
{
    public class LoadingAnimation : MonoBehaviour
    {
        public static Quaternion CashRotation;
        public void Animate()
        {
            transform.DORotate(new Vector3(0, 0, -360), 3).SetRelative()
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

        public void ToggleState(bool state)
        {
            gameObject.SetActive(state);
        }
        private void OnDestroy()
        {
            CashRotation = transform.rotation;
        }
    }
}
