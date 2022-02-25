using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
{
    public class PipetteTool : SelectableTool, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectTool();
        }
    }
}