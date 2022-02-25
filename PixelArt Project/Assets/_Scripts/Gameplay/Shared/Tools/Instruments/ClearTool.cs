using System;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using _Scripts.SharedOverall.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Gameplay.Shared.Tools.Instruments
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
            {
                foreach (var img in DrawingTemplateCreator.ImagesList)
                {
                    img.color = EraserTool.GetColor();
                }
            }
            else
            {
                var i = 0;
                foreach (var img in DrawingTemplateCreator.ImagesList)
                {
                    img.color = LevelCreator.GetPreviousStageScOb().pixelList[i];
                    i++;
                }
            }
        }
    }
}