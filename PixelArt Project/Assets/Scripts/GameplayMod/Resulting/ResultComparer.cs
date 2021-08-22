using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameplayMod.Creating;
using GeneralLogic.Animating;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GeneralLogic.DrawingPanel.DrawingTemplateCreator;

namespace GameplayMod.Resulting
{
    public class ResultComparer : MonoBehaviour
    {
        [SerializeField] private List<Image> stars;
        [SerializeField] private Sprite filledStar;
        [SerializeField] private TextMeshProUGUI resultText;
        private LevelCompleter _levelCompleter;
        private LevelCreator _levelCreator;
        private int _mistakes;

        private void Awake()
        {
            _levelCompleter = FindObjectOfType<LevelCompleter>();
            _levelCreator = FindObjectOfType<LevelCreator>();
        }

        public async void Compare()
        {
            ClipPlaying.SaveResult();
            var rightCount = 0;
            var colorsToCheck = new List<Color32>();
            var listNoWhite = new List<Color32>();

            foreach (var img in PixelImagesList)
            {
                colorsToCheck.Add(img.color);
            }
            foreach (var img in LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList)
            {
                if (img == Color.white) continue;
                listNoWhite.Add(img);
            }

            var count = listNoWhite.Count;
            for (var i = 0; i < PixelImagesList.Count; i++)
            {
                if (colorsToCheck[i].Equals((Color32)LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i]) 
                    && LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i] != Color.white)
                {
                    rightCount++;
                }
                else if (!colorsToCheck[i].Equals((Color32)LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i]) 
                    && LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i] == Color.white)
                {
                    count++;
                }

                if (colorsToCheck[i].Equals((Color32)LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i])) continue;
                PixelImagesList[i].color = Color.red;
                await Task.Delay(200);
                PixelImagesList[i].color = LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].statesList[i];
                await Task.Delay(100);
            }
            var stageMistakes = count - rightCount;
            _mistakes += stageMistakes;
            Debug.Log(_mistakes + " ошибок из " + count);
            CalculateResult(count);
        }

        private void CalculateResult(int count)
        {
            int mark;
            var rights = count - _mistakes;
            var result = (float)rights / count;
            switch (result)
            {
                case var _ when result >= 0.9f:
                    mark = 5;
                    break;
                case var _ when result >= 0.82f && result <= 0.89f:
                    mark = 4;
                    break;
                case var _ when result >= 0.75f && result <= 0.81f:
                    mark = 3;
                    break;
                case var _ when result >= 0.67f && result <= 0.74f:
                    mark = 2;
                    break;
                case var _ when result >= 0.6f && result <= 0.66f:
                    mark = 1;
                    break;
                default:
                    resultText.text = "Уровень не пройден. :(";
                    _levelCompleter.Complete();
                    return;
            }
            Debug.Log(mark);
            
            if (LevelCreator.Stage != LevelCreator.scriptableObject.stageScriptableObjects.Count - 1)
            {
                _levelCreator.Create();
            }
            else
            {
                for (var i = 0; i < mark; i++)
                {
                    stars[i].sprite = filledStar; //Anim
                }
                _levelCompleter.Complete();
            }
        }
    }
}