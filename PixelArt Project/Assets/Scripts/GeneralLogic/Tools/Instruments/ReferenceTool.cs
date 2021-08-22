using GeneralLogic.Tools.Logic;
using UnityEngine.EventSystems;

namespace GeneralLogic.Tools.Instruments
{
    public abstract class ReferenceTool : BaseTool, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
        }
    }
}
