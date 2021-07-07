using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameLogic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class ResultComparer : MonoBehaviour
    {
        [SerializeField] private LevelScriptableObject scriptableObject;
        [SerializeField] private TextMeshProUGUI resultText;
        private List<Color> _colorsToCheck;
        private int _compareCount;
        
        public void Compare()
        {
            _compareCount = 0;
            _colorsToCheck = new List<Color>();

            foreach (var img in LevelTemplateCreator.PixelImagesList)
            {
                _colorsToCheck.Add(img.color);
            }
            
            for (var j = 0; j < LevelTemplateCreator.PixelImagesList.Count; j++)
            {
                if (_colorsToCheck[j] != scriptableObject.statesList[j])
                {
                    _compareCount++;
                }
            }

            StartCoroutine(SetResult());
            Debug.Log(_compareCount + " ошибок");
        }
        
        private IEnumerator SetResult()
        {
            resultText.DOFade(1, 1);
            yield return new WaitForSeconds(1);
        }
    }
}