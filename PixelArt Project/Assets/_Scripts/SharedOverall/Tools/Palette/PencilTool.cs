using _Scripts.GeneralLogic.Tools.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GeneralLogic.Tools.Palette
{
    public abstract class PencilTool : SelectableTool, IPointerClickHandler
    {
        private static Color _pencilColor = Color.black;
        private readonly Color _whiteDeselected = new (1,1,1,0.4f);

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(ToolsManager.Tools.Pencil);
            Select();
        }
        public static void SetColor(Color c)
        {
            _pencilColor = c;
        }
        public static Color GetColor()
        {
            return _pencilColor;
        }

        public override Color GetDeselectedColor()
        {
            if (GetColor().a < 0.3f) return _whiteDeselected;
            return GetColor().CompareRGB(Color.white) ? _whiteDeselected : base.GetDeselectedColor();
        }
    }
}