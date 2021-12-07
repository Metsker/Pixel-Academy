using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;

namespace _Scripts.Gameplay.Recording.ColorPresets
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
