using System;
using _Scripts.Gameplay.Release.Playing.Creating;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall.Audio
{
    public class AudioClickEvent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClick.AudioClickType clickType;
        [SerializeField] private bool ignoreGameState;
        
        public static event Action<AudioClick.AudioClickType> PlaySound;
        
        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                ignoreGameState = true;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play && !LevelCreator.isGameStarted && !ignoreGameState) return;
            PlaySound?.Invoke(clickType);
        }
    }
}