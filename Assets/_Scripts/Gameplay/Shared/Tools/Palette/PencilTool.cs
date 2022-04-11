using _Scripts.Gameplay.Shared.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Palette
{
    public class PencilTool : SelectableTool, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectTool();
        }
    }
}