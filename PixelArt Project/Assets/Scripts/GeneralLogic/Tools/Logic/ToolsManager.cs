using System;
using System.Linq;
using DG.Tweening;
using GeneralLogic.Tools.Instruments;
using GeneralLogic.Tools.Palette;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.Tools.Logic
{
    public class ToolsManager : MonoBehaviour
    {
        public static Tools CurrentTool { get; set; } = Tools.Pencil;
        public static event Action DisablePicker;

        public enum Tools
        {
            Pencil,
            Eraser,
            Filler
        }

        private void OnEnable()
        {
            PencilTool.SetActiveTool += SetActiveTool;
            EraserTool.SetActiveTool += SetActiveTool;
        }

        private void OnDisable()
        {
            PencilTool.SetActiveTool -= SetActiveTool;
            EraserTool.SetActiveTool -= SetActiveTool;
        }

        public static void ResetActiveTool(bool resetColor)
        {
            if (resetColor)
            {
                PencilTool.SetColor(Color.white);
                CurrentTool = Tools.Pencil;
            }
            foreach (var p in FindObjectsOfType<MonoBehaviour>().OfType<ITool>())
            {
                p.GetLine().GetComponent<Image>().color = new Color(1,1,1,0);
            }
            FindObjectOfType<FillingTool>().ResetColor();
            DisablePicker?.Invoke();
            
        }
        private void SetActiveTool(Image i)
        {
            foreach (var p in FindObjectsOfType<MonoBehaviour>().OfType<ITool>())
            {
                if (p.GetLine().GetComponent<Image>() == i)
                {
                    i.DOFade(1, 0);
                    continue;
                }
                p.GetLine().GetComponent<Image>().DOFade(0, 0);
            }
        }
    }
}
