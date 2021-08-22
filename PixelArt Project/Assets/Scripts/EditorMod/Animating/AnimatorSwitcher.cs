using System.Collections.Generic;
using GeneralLogic;
using GeneralLogic.Animating;
using GeneralLogic.DrawingPanel;
using GeneralLogic.Tools;
using GeneralLogic.Tools.Logic;
using UnityEngine;

namespace EditorMod.Animating
{
    public class AnimatorSwitcher : ClipPlaying
    {
        public void SwitchAnimate()
        {
            if (GameModManager.CurrentGameMod != GameModManager.GameMod.Record) return;
            switch (animator.isActiveAndEnabled)
            {
                case true :
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                    animator.enabled = false;
                    progress.value = 0;
                    LoadState();
                    ToolsManager.ResetActiveTool(true);
                    break;
                case false :
                    if (ClipListLoader.AnimationClips.Count == 0)
                    {
                        Debug.LogWarning("Создайте новый клип");
                        break;
                    }
                    if(ClipListLoader.ClipNumber==ClipListLoader.AnimationClips.Count) return;
                    if (DrawingTemplateCreator.PixelImagesList == null)
                    {
                        Debug.LogWarning("Создайте сетку");
                        return;
                    }
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
                    SaveState();
                    animator.Rebind();
                    ChangeClip((AnimationClip)ClipListLoader.AnimationClips[ClipListLoader.ClipNumber]);
                    animator.enabled = true;
                    break;
            }
        }
    }
}