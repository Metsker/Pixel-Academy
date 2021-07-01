using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class ToolMod : MonoBehaviour
    {
        private Color UnselectedColor { get; } = new Color(1, 1, 1, 0.3f);
        private Color SelectedColor { get; } = new Color(0.45f,1,0.6f,0.6f);
        public static Tools CurrentTool = Tools.Pencil;
        [SerializeField] private GameObject[] toolBoxes;

        public enum Tools
        {
            Pencil,
            Eraser
        }

        public void ResetBoxColors()
        {
            foreach (var box in toolBoxes)
            {
                box.GetComponent<Image>().color = UnselectedColor;
            }
        }

        public void SetBoxColor(GameObject g)
        {
            g.GetComponent<Image>().color = SelectedColor;
        }
    }
}
