using System;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
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