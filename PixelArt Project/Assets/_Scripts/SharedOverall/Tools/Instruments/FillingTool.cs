using _Scripts.GeneralLogic.Tools.Logic;
using _Scripts.GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GeneralLogic.Tools.Instruments
{
    public class FillingTool : SelectableTool, IPointerClickHandler
    {
        private static Color _fillingColor = Color.white;

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (ToolsManager.CurrentTool)
            {
                case ToolsManager.Tools.Pencil when PencilTool.GetColor() != GetDeselectedColor():
                    ClickEvent(ToolsManager.Tools.Filler);
                    SetColor(PencilTool.GetColor());
                    Select();
                    break;
            }
        }
        private void SetColor(Color c)
        {
            _fillingColor = c;
        }
        
        public override bool IsSelected()
        {
            return GetSelectImage().color != GetDeselectedColor();
        }
        
        public static Color GetColor()
        {
            return _fillingColor;
        }
        protected override Color GetSelectedColor()
        {
            return GetColor();
        }
    }
}
