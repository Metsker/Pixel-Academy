using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Palette
{
    public class EraserTool: SelectableTool, IPointerClickHandler
    {
        private static readonly Color EraseColor = Color.white;
        private readonly Color _deselectedColor = new (1,1,1,0.4f);

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(ToolsManager.Tools.Eraser);
            Select();
        }
        public override Color GetDeselectedColor()
        {
            return _deselectedColor;
        }
        public static Color GetColor()
        {
            return EraseColor;
        }
        public static Color GetColor(int index)
        {
            return LevelCreator.GetPreviousStageScOb().pixelList[index];
        }
    }
}