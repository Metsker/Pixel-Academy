using System;
using _Scripts.GameplayMod.Creating;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.DrawingPanel;
using _Scripts.GeneralLogic.Tools.Logic;
using _Scripts.GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GeneralLogic.Tools.Instruments
{
    public class ClearTool : BaseTool, IPointerClickHandler
    {
        public static event Action<WarningUI.WarningType> ShowWarning;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Record)
            {
                Clear();
                return;
            }
            if (PlayerPrefs.GetInt("ClearWarning", 0) == 1)
            {
                Clear();
            }
            else
            {
                ShowWarning?.Invoke(WarningUI.WarningType.Clear);
            }
        }
        
        public static void Clear()
        {
            if (DrawingTemplateCreator.ImagesList == null) return;
            if (LevelCreator.Stage <= 0)
                for (var i = 0; i < DrawingTemplateCreator.PixelList.Count; i++)
                {
                    DrawingTemplateCreator.ImagesList[i].color = EraserTool.GetColor();
                }
            else
                for (var i = 0; i < DrawingTemplateCreator.PixelList.Count; i++)
                {
                    DrawingTemplateCreator.ImagesList[i].color = LevelCreator.GetPreviousStageScOb().pixelList[i];
                }
        }
    }
}