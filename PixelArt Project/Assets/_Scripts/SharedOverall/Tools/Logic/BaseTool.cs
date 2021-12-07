using System;
using _Scripts.GameplayMod.Creating;
using _Scripts.GeneralLogic.Animating;
using UnityEngine;
using static _Scripts.GeneralLogic.ColorPresets.PickerHandler;

namespace _Scripts.GeneralLogic.Tools.Logic
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