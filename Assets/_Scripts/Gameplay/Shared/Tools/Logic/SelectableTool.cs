using System;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Shared.Tools.Logic
{
    public abstract class SelectableTool : BaseTool, ISelectable
    {
        public ToolsManager.Tools toolType;
        private Image selectImage;
        
        private readonly Color _selectedColor = Color.black;
        private readonly Color _deselectedColor = new (0,0,0,0.4f);
        public static event Action StartLevel;

        protected new void Awake()
        {
            base.Awake();
            selectImage = transform.GetChild(0).GetComponent<Image>();
        }
        
        public void SelectWithoutAnimation()
        {
            TryStartLevel();
            OnSelect();
            GetSelectImage().color = GetSelectedColor();
        }

        protected void SelectTool()
        {
            if (ToolsManager.CurrentTool != toolType)
            {
                ClickEvent(toolType);
                SelectWithoutAnimation();
                
            }
            else if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording)
            {
                TryStartLevel();
                PlayAnimation();
                PickerHandler.DisablePicker();
            }
        }
        public void SelectToolNoStates()
        {
            if (ToolsManager.CurrentTool != toolType)
            {
                ToolsManager.CurrentTool = toolType;
                SelectWithoutAnimation();
            }
            PlayAnimation();
        }

        private void TryStartLevel()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play ||
                GameStateManager.CurrentGameState == GameStateManager.GameState.Drawing) return;
            StartLevel?.Invoke();
        }
        public void Deselect()
        {
            GetSelectImage().color = GetDeselectedColor();
        }
        public virtual bool IsSelected()
        {
            return GetSelectImage().color == GetSelectedColor();
        }
        public Image GetSelectImage()
        {
            return selectImage;
        }
        public virtual Color GetDeselectedColor()
        {
            return _deselectedColor;
        }
        private Color GetSelectedColor()
        {
            return _selectedColor;
        }
        protected virtual void OnSelect()
        {
            ToolsManager.DeselectTools();
        }
    }
}