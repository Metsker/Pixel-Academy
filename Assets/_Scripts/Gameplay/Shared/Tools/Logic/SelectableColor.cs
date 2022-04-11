
using _Scripts.Gameplay.Shared.Tools.Logic;

namespace _Scripts.SharedOverall.Tools.Logic
{
    public abstract class SelectableColor : SelectableTool
    {
        protected override void OnSelect()
        {
            ToolsManager.DeselectColors();
        }
    }
}