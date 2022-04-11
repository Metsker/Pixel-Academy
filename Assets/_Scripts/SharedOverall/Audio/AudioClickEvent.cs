using System;
using _Scripts.Gameplay.Playing.Creating;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall.Audio
{
    public class AudioClickEvent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioManager.AudioClickType clickType;
        [SerializeField] private bool ignoreGameState;
        
        public static event Action<AudioManager.AudioClickType> PlaySound;
        
        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                ignoreGameState = true;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play && !LevelCreator.IsGameStarted && !ignoreGameState) return;
            PlaySound?.Invoke(clickType);
        }
    }
}