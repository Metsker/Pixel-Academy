using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelTemplateCreator : MonoBehaviour
    {
        public static readonly List<Image> PixelImagesList = new List<Image>();
        
        public static void CreateList()
        {
            foreach (var img in FindObjectsOfType<ClickOnPixel>())
            {
                PixelImagesList.Add(img.GetComponent<Image>());
            }
        }
    }
}
