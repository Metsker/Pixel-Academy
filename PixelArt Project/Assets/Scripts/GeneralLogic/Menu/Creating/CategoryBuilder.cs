using System.Collections;
using System.Collections.Generic;
using EditorMod.ScriptableObjectLogic;
using GameplayMod.Animating;
using GameplayMod.Data;
using GameplayMod.Transition;
using GameplayMod.UI;
using GeneralLogic;
using GeneralLogic.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayMod.Creating
{
    public class CategoryBuilder : MonoBehaviour, IBuilder
    {
        [SerializeField] protected List<LevelScriptableObject> levels;
        [SerializeField] private TextMeshProUGUI label;
        
        protected CreatingData creatingData;
        private StageAnimation _stageAnimation;
        
        private Vector2 _rectStartSize;

        private void Awake()
        {
            creatingData = FindObjectOfType<CreatingData>();
            _stageAnimation = FindObjectOfType<StageAnimation>();
        }

        protected void Start()
        {
            var g = creatingData.categoryInstance;
            var data = g.GetComponent<LevelData>();
            _rectStartSize = data.previewRect.sizeDelta;
        }

        public void OpenCategory()
        {
            creatingData.label.text = label.text;
            StartCoroutine(Build());
        }
        
        public IEnumerator Build()
        {
            var childCount = creatingData.levelPanel.transform.childCount;
            if (childCount < levels.Count)
            {
                var c = levels.Count - childCount;
                for (var i = 0; i < c; i++)
                {
                    var g = Instantiate(creatingData.categoryInstance, creatingData.levelPanel.transform);
                    g.name = "Level " + i;
                }
                yield return null;
            }
            else if (childCount > levels.Count)
            {
                var c = childCount - levels.Count;
                for (var i = c - 1; i >= 0; i--)
                {
                    Destroy(creatingData.levelPanel.transform.GetChild(i).gameObject);
                }
                yield return null;
            }
            LoadChildren(creatingData.levelPanel);
            _stageAnimation.ShowLevels();
        }

        protected void LoadChildren(GameObject panel)
        {
            for (var j = 0; j < panel.transform.childCount; j++)
            {
                var g = panel.transform.GetChild(j);
                var data = g.GetComponent<LevelData>();
                
                if (data.preview.sprite == levels[j].previewSprite) continue;
                ImageAdjuster.Adjust(data.previewRect, levels[j].previewSprite, _rectStartSize);
                data.preview.sprite = levels[j].previewSprite;
                data.scriptableObject = levels[j];

                var isCompleted = data.scriptableObject.stars > 0;
                data.state.gameObject.SetActive(isCompleted);
                data.stars[0].transform.parent.gameObject.SetActive(isCompleted);
                if (isCompleted)
                {
                    foreach (var t in data.stars)
                    {
                        if (t.sprite == creatingData.unfilledStar) continue;
                        t.sprite = creatingData.unfilledStar;
                    }
                    for (var i = 0; i < data.scriptableObject.stars; i++)
                    {
                        data.stars[i].sprite = creatingData.filledStar;
                    }
                }

                if (DifficultyFilterButton.currentDifficulty != DifficultyFilterButton.Difficulties.None
                    && data.scriptableObject.difficulty != DifficultyFilterButton.currentDifficulty)
                {
                    g.gameObject.SetActive(false);
                }
            }
        }
    }
}
