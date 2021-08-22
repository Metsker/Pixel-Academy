using System;
using GeneralLogic.Tools.Logic;
using GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Instruments
{
    public class FillingTool : BaseTool, IPointerClickHandler
    {
        [SerializeField] private Image fillingImage;

        private void OnEnable()
        {
            ResetFillingColor += ResetColor;
        }
        private void OnDisable()
        {
            ResetFillingColor -= ResetColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(ToolsManager.Tools.Filler);
            ToolsManager.ResetActiveTool(false);
            fillingImage.color = PencilTool.GetColor();
        }

        public void ResetColor()
        {
            if (ToolsManager.CurrentTool == ToolsManager.Tools.Filler) return;
            fillingImage.color = Color.white;
            PencilTool.SetColor(Color.white);
        }
    }
}
