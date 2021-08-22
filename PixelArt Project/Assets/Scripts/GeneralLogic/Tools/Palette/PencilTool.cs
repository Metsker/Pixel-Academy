using System;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Palette
{
    public abstract class PencilTool : BaseTool, ITool, IPointerClickHandler
    {
        [SerializeField] private Image line;
        private static Color _pencilColor = Color.white;
        public static event Action<Image> SetActiveTool;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(SetActiveTool, GetLine(), ToolsManager.Tools.Pencil);
        }
        
        public static void SetColor(Color c)
        {
            _pencilColor = c;
        }
        public static Color GetColor()
        {
            return _pencilColor;
        }
        public Image GetLine()
        {
            return line;
        }
    }
}