using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Recording;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class LevelListManager : MonoBehaviour
    {
        [SerializeField] private SaveLevelScriptableObject scriptableObject;
        public static readonly List<Image> PixelImagesList = new List<Image>();
        private List<Color> _colorsToCheck;
        private int _compareCount;

        public static void CreateList()
        {
            foreach (var img in FindObjectsOfType<ClickOnPixel>())
            {
                PixelImagesList.Add(img.GetComponent<Image>());
            }
        }

        public void Check()
        {
            _compareCount = 0;
            _colorsToCheck = new List<Color>();

            foreach (var img in PixelImagesList)
            {
                _colorsToCheck.Add(img.color);
            }
            
            for (var j = 0; j < PixelImagesList.Count; j++)
            {
                if (_colorsToCheck[j] != scriptableObject.statesList[j])
                {
                    _compareCount++;
                }
            }
            Debug.Log(_compareCount);
        }
    }
}
