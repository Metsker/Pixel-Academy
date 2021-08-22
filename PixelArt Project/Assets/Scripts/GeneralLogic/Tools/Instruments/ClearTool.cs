using GeneralLogic.DrawingPanel;
using GeneralLogic.Tools.Logic;
using GeneralLogic.Tools.Palette;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Instruments
{
    public class ClearTool : BaseTool, ICleaner, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            Clear();
        }
        
        public void Clear()
        {
            if (DrawingTemplateCreator.PixelImagesList == null) return;
            foreach (var img in DrawingTemplateCreator.PixelImagesList)
            {
                img.GetComponent<Image>().color = EraserTool.GetColor();
            }
        }
    }
}