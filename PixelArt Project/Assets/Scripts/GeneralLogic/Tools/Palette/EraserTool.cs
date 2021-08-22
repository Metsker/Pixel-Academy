using System;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Palette
{
    public class EraserTool: BaseTool, IPointerClickHandler, ITool
    {
        [SerializeField] private Image line;
        private static readonly Color EraseColor = Color.white;
        public static event Action<Image> SetActiveTool;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(SetActiveTool, GetLine(), ToolsManager.Tools.Eraser);
        }

        public static Color GetColor()
        {
            return EraseColor;
        }

        public Image GetLine()
        {
            return line;
        }
    }
}