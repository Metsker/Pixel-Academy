using System;
using GeneralLogic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EditorMod.Recording
{
    public class ScrollRecording : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        public static event Action<float> TakeSnapshot;
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
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                TakeSnapshot?.Invoke(Time.deltaTime); 
            }
        }
    }
}
