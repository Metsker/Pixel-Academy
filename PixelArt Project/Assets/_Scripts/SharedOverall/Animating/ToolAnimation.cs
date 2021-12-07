using System.Collections;
using _Scripts.Gameplay.Recording.Recording;
using _Scripts.SharedOverall.Audio;
using DG.Tweening;
using UnityEngine;
#if (UNITY_EDITOR)

#endif

namespace _Scripts.SharedOverall.Animating
{
    public class ToolAnimation : MonoBehaviour
    {
        private bool _isAnimating;
        public static bool isAnyAnimating { get; private set; }
        public const float AnimDuration = 0.6f;

        public void PlayAnimation()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            isAnyAnimating = true;
            
            transform.DOPunchScale(Vector3.one/3, AnimDuration,1).OnComplete(()=>
            {
                _isAnimating = false;
                isAnyAnimating = false;
            });
#if (UNITY_EDITOR)
            if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording) return;
            StartCoroutine(AnimationRecording());
#endif
        }
        
#if (UNITY_EDITOR)
        private IEnumerator AnimationRecording()
        {
            Recorder.Snapshot(Recorder.SnapshotDelay/2);
            Recorder.Snapshot(AudioClick.AudioClickType.Tool);
            
            for (float i = 0; i < AnimDuration; i+=Time.deltaTime)
            {
                Recorder.Snapshot(Time.deltaTime);
                yield return null;
            }
        }
#endif
    }
}