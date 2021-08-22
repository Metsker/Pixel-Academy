using System.Collections.Generic;
using System.Linq;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.DrawingPanel
{
    public class DrawingTemplateCreator : MonoBehaviour, ICreator
    {
        public static List<Image> PixelImagesList { get; private set; }

        public void Create()
        {
            PixelImagesList = new List<Image>();
            foreach (var img in FindObjectsOfType<ClickOnPixel>().Reverse())
            {
                PixelImagesList.Add(img.GetComponent<Image>());
            }
        }
    }
}
