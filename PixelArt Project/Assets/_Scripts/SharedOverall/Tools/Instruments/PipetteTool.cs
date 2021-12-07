using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
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