#if (UNITY_EDITOR)
using _Scripts.SharedOverall;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Recording.Recording
{
    public class ScrollRecording : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        
        private UnityAction<Vector2> _scrollAction;

        private void Awake()
        {
            _scrollAction = Snapshot;
        }

        private void OnEnable()
        {
            scroll.onValueChanged.AddListener(_scrollAction);
        }
        private void OnDisable()
        {
            scroll.onValueChanged.RemoveListener(_scrollAction);
        }
    
        private void Snapshot(Vector2 value)
        {
            if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording) return;
            Recorder.Snapshot(Time.deltaTime);
        }
    }
}
#endif