using System.Collections.Generic;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.SharedOverall.Animating;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Playing.Resulting.UI
{
    public class ResultView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = new Color(1, 1, 1, 0.5f);
            CompareResult(LevelCreator.GetCurrentStageScOb().pixelList);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = Color.white;
            CompareResult(ClipPlaying.CashImageStates);
        }
        
        private void CompareResult(List<Color> colors)
        {
            for (var i = 0; i < LevelCompleter.ResultPixels.Count; i++)
            {
                if(LevelCompleter.ResultPixels[i].color == colors[i]) continue;
                LevelCompleter.ResultPixels[i].color = colors[i];
            }
        }
    }
}