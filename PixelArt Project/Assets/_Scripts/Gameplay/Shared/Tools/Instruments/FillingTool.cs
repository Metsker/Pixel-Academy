using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Instruments
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
                case ToolsManager.Tools.Eraser when GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play:
                    ClickEvent(ToolsManager.Tools.Filler);
                    SetColor(EraserTool.GetColor());
                    Select();
                    break;
                default:
                    ClickEventNoStates();
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
