using System;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Animating;
using UnityEngine;
using static _Scripts.SharedOverall.ColorPresets.PickerHandler;

namespace _Scripts.SharedOverall.Tools.Logic
{
    public abstract class BaseTool: MonoBehaviour
    {
        protected ToolAnimation toolAnimation;

        protected void Awake()
        {
            toolAnimation = GetComponentInParent<ToolAnimation>();
        }

        protected void ClickEvent(ToolsManager.Tools tool)
        {
            BaseStates();
            ToolsManager.CurrentTool = tool;
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play || tool == ToolsManager.Tools.Pencil) return;
            DisablePicker();
        }
        
        protected void ClickEvent()
        {
            BaseStates();
            DisablePicker();
        }

        protected void ClickEventNoStates()
        {
            toolAnimation.PlayAnimation();
            DisablePicker();
        }
        
        private void BaseStates()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording && ToolAnimation.isAnyAnimating)
                throw new Exception("Wait for animation");
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play &&
                !LevelCreator.isGameStarted) throw new Exception("Game isn't started");
            toolAnimation.PlayAnimation();
        }
    }
}