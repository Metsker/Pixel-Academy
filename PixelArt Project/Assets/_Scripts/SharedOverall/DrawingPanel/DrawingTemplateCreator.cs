using System.Collections.Generic;
using System.Linq;
using _Scripts.GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GeneralLogic.DrawingPanel
{
    public class DrawingTemplateCreator : MonoBehaviour, ICreator
    {
        public static List<Image> ImagesList { get; private set; }
        public static List<ClickOnPixel> PixelList { get; private set; }

        public void Create()
        {
            ImagesList = new List<Image>();
            PixelList = new List<ClickOnPixel>();
            foreach (var clk in FindObjectsOfType<ClickOnPixel>().Reverse())
            {
                PixelList.Add(clk);
                ImagesList.Add(clk.GetComponent<Image>());
                clk.SetColliderSize();
            }
        }
    }
}