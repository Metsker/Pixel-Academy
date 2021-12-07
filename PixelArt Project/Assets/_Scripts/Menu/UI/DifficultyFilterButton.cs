using _Scripts.GeneralLogic.Menu.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _Scripts.GeneralLogic.Menu.UI.DifficultyFilterManager;

namespace _Scripts.GeneralLogic.Menu.UI
{
    public class DifficultyFilterButton : MonoBehaviour, IPointerClickHandler
    {
        public Difficulties difficulty;
        [SerializeField] private Image shape;
        [SerializeField] private Sprite selectedSprite;

        private DifficultyFilterManager _difficultyFilterManager;
        private Sprite _unselectedSprite;
        
        private void Awake()
        {
            _unselectedSprite = shape.sprite;
            _difficultyFilterManager = GetComponentInParent<DifficultyFilterManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (difficulty == currentDifficulty)
            {
                Switch(Difficulties.None);
                return;
            }
            switch (difficulty)
            {
                case Difficulties.Easy:
                    Switch(Difficulties.Easy);
                    break;
                case Difficulties.Medium:
                    Switch(Difficulties.Medium);
                    break;
                case Difficulties.Hard:
                    Switch(Difficulties.Hard);
                    break;
            }
        }

        public void Switch(Difficulties dif)
        {
            foreach (var difficultyFilter in _difficultyFilterManager.filters)
            {
                if (difficultyFilter == this && dif != Difficulties.None)
                {
                    shape.sprite = selectedSprite;
                    continue;
                }
                difficultyFilter.shape.sprite = _unselectedSprite;
            }
            
            foreach (var data in FindObjectsOfType<LevelData>(true))
            {
                if (data.gameObject.CompareTag("Released")) continue;
                
                if (dif == Difficulties.None)
                {
                    data.gameObject.SetActive(true);
                    continue;
                }

                if (data.scriptableObject != null)
                {
                    data.gameObject.SetActive(data.scriptableObject.difficulty == dif);
                }
            }
            currentDifficulty = dif;
        }
    }
}
