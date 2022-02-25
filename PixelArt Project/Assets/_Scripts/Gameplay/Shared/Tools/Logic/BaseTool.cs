using System;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using UnityEngine;
using static _Scripts.SharedOverall.ColorPresets.PickerHandler;

namespace _Scripts.Gameplay.Shared.Tools.Logic
{
    public abstract class BaseTool: MonoBehaviour
    {
        private ToolAnimation toolAnimation;
        public static event Action<TextHint.HintType, float> ShowHint;

        protected void Awake()
        {
            toolAnimation = GetComponentInParent<ToolAnimation>();
        }

        protected void ClickEvent(ToolsManager.Tools tool)
        {
            BaseStates();
            ToolsManager.CurrentTool = tool;
            PlayAnimation();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play || tool == ToolsManager.Tools.Pencil) return;
            DisablePicker();
        }

        protected void ClickEvent()
        {
            BaseStates();
            DisablePicker();
            PlayAnimation();
        }

        protected void PlayAnimation()
        {
            toolAnimation.PlayAnimation();
        }

        protected virtual void BaseStates()
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play && !ClipManager.IsHintPlayed) throw new Exception("Wait for first hint");
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording && ToolAnimation.isAnyAnimating)
                throw new Exception("Wait for animation");
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play &&
                GameStateManager.CurrentGameState != GameStateManager.GameState.Drawing &&
                !ClipManager.IsReadyToStart)
            {
                ShowHint?.Invoke(TextHint.HintType.BaseStates, 2);
                throw new Exception("Game isn't started");
            }
        }
    }
}