using _Scripts.Gameplay.Shared.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
{
    public class FillingTool : SelectableTool, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectTool();
        }
    }
}
