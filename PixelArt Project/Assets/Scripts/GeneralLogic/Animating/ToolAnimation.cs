using System;
using System.Collections;
using DG.Tweening;
using EditorMod.Recording;
using UnityEngine;

namespace GeneralLogic.Animating
{
    public class ToolAnimation : MonoBehaviour
    {
        private const float AnimDuration = 0.6f;
        public static bool isAnyAnimPlaying { get; private set; }
        public bool isThisAnimPlaying { get; private set; }
        public static event Action<float> TakeSnapshot;

        public void PlayAnim()
        {
            StartCoroutine(UIAnim());
        }
        
        private IEnumerator UIAnim()
        {
            isAnyAnimPlaying = true;
            isThisAnimPlaying = true;
            TakeSnapshot?.Invoke(Recorder.SnapshotDelay/2); //Test
            transform.DOPunchScale(Vector3.one/3, AnimDuration,1).OnComplete(()=>
            {
                isAnyAnimPlaying = false;
                isThisAnimPlaying = false;
            });
            if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording) yield break;
            for (float i = 0; i < AnimDuration; i+=Time.deltaTime)
            {
                TakeSnapshot?.Invoke(Time.deltaTime);
                yield return null;
            }
        }
    }
}