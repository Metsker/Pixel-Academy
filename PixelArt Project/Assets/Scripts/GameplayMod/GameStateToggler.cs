using System.Threading.Tasks;
using GameplayMod.Creating;
using GeneralLogic;
using GeneralLogic.Animating;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayMod
{
    public class GameStateToggler : ClipPlaying
    {
        [SerializeField] private Button submitButton;
        public static bool isGameStarted { get; set; }
        private bool _animatorState;

        private void Start()
        {
            isGameStarted = false;
        }

        public async void SetClip()
        {
            ChangeClip(LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].animationClip);
            await Task.Yield(); //?
            PlayClip();
            await Task.Yield(); //?
            SaveState();
            isGameStarted = false;
        }
        
        public void PlayClip()
        {
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.Rebind();
            animator.enabled = true;
            progress.gameObject.SetActive(true);
            submitButton.interactable = false;
        }
        
        public void StopClip()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) &&
                !isGameStarted)
            {
                isGameStarted = true;
                submitButton.interactable = true;
                GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                animator.enabled = false;
                ToolsManager.ResetActiveTool(true);
                progress.gameObject.SetActive(false);
                LoadState();
            }
            else
            {
                animator.enabled = _animatorState;
                _animatorState = !_animatorState;
            }
        }
    }
}
