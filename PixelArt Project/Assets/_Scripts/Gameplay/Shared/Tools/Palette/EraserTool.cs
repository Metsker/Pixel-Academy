using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.SharedOverall.Tools.Palette
{
    public class EraserTool: SelectableTool, IPointerClickHandler
    {
        private static readonly Color EraseColor = Color.white;
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectTool();
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