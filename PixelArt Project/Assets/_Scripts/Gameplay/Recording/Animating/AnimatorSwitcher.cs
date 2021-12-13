using System.Collections;
using System.Threading.Tasks;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Recording.Animating
{
    public abstract class AnimatorSwitcher : ClipPlaying
    {
        [SerializeField] private Button copyButton;

        public void SwitchAnimate(bool isCopy)
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record) return;
            switch (animator.isActiveAndEnabled)
            {
                case true :
                    if (clipTimer != null)
                    {
                        StopCoroutine(clipTimer);
                    }
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                    animator.enabled = false;
                    progress.value = 0;
                    ToolsManager.CurrentTool = ToolsManager.Tools.None;
                    ToolsManager.DeselectTools();
                    copyButton.gameObject.SetActive(false);
                    if (!isCopy)
                    {
                        LoadState();
                    }
                    break;
                case false :
                    if (ClipListLoader.AnimationClips.Count == 0)
                    {
                        Debug.LogWarning("Создайте первый клип");
                        break;
                    }
                    if(ClipListLoader.ClipNumber==ClipListLoader.AnimationClips.Count) return;
                    if (DrawingTemplateCreator.ImagesList == null)
                    {
                        Debug.LogWarning("Создайте сетку");
                        return;
                    }
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
                    copyButton.gameObject.SetActive(true);
                    SaveState();
                    ChangeClip(ClipListLoader.GetCurrentClip());
                    animator.Play(0, 0, 0);
                    animator.enabled = true;
                    break;
            }
        }

        public async void CopyState()
        {
            animator.Play(0, 0, 1);
            await Task.Delay(50);
            SwitchAnimate(true);
        }
        protected override IEnumerator ClipTimer()
        {
            yield return new WaitForSeconds(StartDelaySeconds);
            if (animator.enabled == false) yield break;
            SwitchAnimate(false);
        }
    }
}