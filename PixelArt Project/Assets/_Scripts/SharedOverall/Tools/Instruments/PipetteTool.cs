using _Scripts.GeneralLogic.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.GeneralLogic.Tools.Instruments
{
    public class PipetteTool : BaseTool, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (ToolsManager.CurrentTool == ToolsManager.Tools.Pencil)
            {
                ClickEvent(ToolsManager.Tools.Pipette);
            }
        }
    }
}