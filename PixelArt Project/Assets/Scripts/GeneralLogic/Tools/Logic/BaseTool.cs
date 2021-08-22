using System;
using GameplayMod;
using GeneralLogic.Animating;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Logic
{
    public abstract class BaseTool: MonoBehaviour
    {
        private ToolAnimation _toolAnimation;
        protected static event Action ResetFillingColor;
        
        protected void Awake()
        {
            _toolAnimation = GetComponentInParent<ToolAnimation>();
        }

        protected void ClickEvent(Action<Image> action, Image line, ToolsManager.Tools tool)
        {
            BaseStates();
            action?.Invoke(line);
            ToolsManager.CurrentTool = tool;
            ResetFillingColor?.Invoke();
        }
        
        protected void ClickEvent(ToolsManager.Tools tool)
        {
            BaseStates();
            ToolsManager.CurrentTool = tool;
            ResetFillingColor?.Invoke();
        }
        
        protected void ClickEvent()
        {
            BaseStates();
        }

        private void BaseStates()
        {
            if (_toolAnimation.isThisAnimPlaying) throw new Exception("Wait for animation");
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording && ToolAnimation.isAnyAnimPlaying)
                throw new Exception("Wait for animation");
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play &&
                !GameStateToggler.isGameStarted) throw new Exception("Game isn't started");
            _toolAnimation.PlayAnim();
        }
    }
}