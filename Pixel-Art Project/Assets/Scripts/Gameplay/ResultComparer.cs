using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapEditor.Recording;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class ResultChecker : MonoBehaviour
    {
        [SerializeField] private SaveLevelScriptableObject scriptableObject;
        [SerializeField] private TextMeshProUGUI resultText;
        private List<Color> _colorsToCheck;
        private int _compareCount;
        
        public void Check()
        {
            _compareCount = 0;
            _colorsToCheck = new List<Color>();

            foreach (var img in PixelListCreator.PixelImagesList)
            {
                _colorsToCheck.Add(img.color);
            }
            
            for (var j = 0; j < PixelListCreator.PixelImagesList.Count; j++)
            {
                if (_colorsToCheck[j] != scriptableObject.statesList[j])
                {
                    _compareCount++;
                }
            }

            StartCoroutine(SetResult());
            Debug.Log(_compareCount + "ошибок");
        }
        
        private IEnumerator SetResult()
        {
            resultText.DOFade(1, 1);
            yield return new WaitForSeconds(1);
        }
    }
}