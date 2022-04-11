#if (UNITY_EDITOR)
using _Scripts.SharedOverall;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Recording.Recording
{
    public class ScrollRecorder : MonoBehaviour
    {
        private ScrollRect _scroll;
        
        private UnityAction<Vector2> _scrollAction;

        private void Awake()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record)
            {
                Destroy(this);
                return;
            }
            _scroll = GetComponent<ScrollRect>();
            _scrollAction = Snapshot;
        }

        private void OnEnable()
        {
            if (_scrollAction == null) return;
         
            _scroll.onValueChanged.AddListener(_scrollAction);
        }
        private void OnDisable()
        {
            if (_scrollAction == null) return;

            _scroll.onValueChanged.RemoveListener(_scrollAction);
        }
    
        private void Snapshot(Vector2 value)
        {
            if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording) return;
            Recorder.Snapshot(Time.deltaTime);
        }
    }
}
#endif