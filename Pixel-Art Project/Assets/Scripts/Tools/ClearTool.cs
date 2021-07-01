using Gameplay;
using MapEditor.PresetSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class ClearTool : MonoBehaviour
    {
        public void Clear()
        {
            foreach (var img in LevelListManager.PixelImagesList)
            {
                img.GetComponent<Image>().color = FindObjectOfType<EraserTool>().GetColor();
            }
        }
    }
}
