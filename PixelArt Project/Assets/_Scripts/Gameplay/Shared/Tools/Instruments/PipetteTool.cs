using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
{
    public class PipetteTool : SelectableTool, IPointerClickHandler
    {
        private readonly Color _deselectedColor = ToolsManager.ColorZero;
        private readonly Color _selectedColor = new(0,0,0,1);
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (ToolsManager.CurrentTool == ToolsManager.Tools.Pencil)
            {
                ClickEvent(ToolsManager.Tools.Pipette);
                Select();
            }
            else
            {
                ClickEventNoStates();
            }
        }
        
        public override Color GetDeselectedColor()
        {
            return _deselectedColor;
        }
        protected  override Color GetSelectedColor()
        {
            return _selectedColor;
        }
    }
}