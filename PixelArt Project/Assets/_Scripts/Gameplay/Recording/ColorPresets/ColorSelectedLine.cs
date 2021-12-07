using _Scripts.GeneralLogic.Tools.Logic;
using UnityEngine;

namespace _Scripts.EditorMod.ColorPresets
{
    public class ColorSelectedLine : MonoBehaviour
    {
        private void OnDestroy()
        {
            if (!gameObject.activeSelf) return;
            ToolsManager.CurrentTool = ToolsManager.Tools.None;
        }
    }
}
