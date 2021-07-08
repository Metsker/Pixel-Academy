using System;
using System.Collections.Generic;
using MapEditor.ColorPresets;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelTemplateCreator : MonoBehaviour
    {
        public static readonly List<Image> PixelImagesList = new List<Image>();
        public static readonly List<GameObject> ColorPresetsList = new List<GameObject>();

        public static void CreatePixelList()
        {
            foreach (var img in FindObjectsOfType<ClickOnPixel>())
            {
                PixelImagesList.Add(img.GetComponent<Image>());
            }
        }
        public static void CreateColorPresetList()
        {
            foreach (var pr in FindObjectsOfType<ColorPreset>())
            {
                ColorPresetsList.Add(pr.gameObject);
            }
        }
    }
}
