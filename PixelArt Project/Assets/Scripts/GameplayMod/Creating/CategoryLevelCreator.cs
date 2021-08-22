using System;
using System.Collections;
using System.Collections.Generic;
using EditorMod.ScriptableObjectLogic;
using GameplayMod.UI;
using GeneralLogic;
using GeneralLogic.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayMod.Creating
{
    public class CategoryLevelCreator : MonoBehaviour, IBuilder
    {
        [SerializeField] private List<LevelScriptableObject> levels;
        private LevelCreationData _levelCreationData;
        private StageAnimation _stageAnimation;
        private const string PreviewName = "Preview";
        private Vector2 _rectSize;

        private void Awake()
        {
            _levelCreationData = FindObjectOfType<LevelCreationData>();
            _stageAnimation = FindObjectOfType<StageAnimation>();
        }

        private void Start()
        {
            var g = _levelCreationData.categoryInstance;
            var data = g.GetComponent<LevelData>();
            _rectSize = data.previewRect.sizeDelta;
        }

        public void StartBuild()
        {
            StartCoroutine(Build());
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator Build()
        {
            var childCount = _levelCreationData.levelPanel.transform.childCount;
            if (childCount < levels.Count)
            {
                var c = levels.Count - childCount;
                for (var i = 0; i < c; i++)
                {
                    Instantiate(_levelCreationData.categoryInstance, _levelCreationData.levelPanel.transform);
                }
                yield return null;
            }
            else if (childCount > levels.Count)
            {
                var c = childCount - levels.Count;
                for (var i = c - 1; i >= 0; i--)
                {
                    Destroy(_levelCreationData.levelPanel.transform.GetChild(i).gameObject);
                }
                yield return null;
            }
            
            for (var j = 0; j < _levelCreationData.levelPanel.transform.childCount; j++)
            {
                var g = _levelCreationData.levelPanel.transform.GetChild(j);
                var data = g.GetComponent<LevelData>();
                if (data.preview.sprite == levels[j].previewSprite) continue;
                var rt = data.previewRect;
                if (rt.sizeDelta != _rectSize)
                {
                    rt.sizeDelta = _rectSize;
                }
                ImageAdjuster.Adjust(data.previewRect, levels[j].previewSprite);
                data.preview.sprite = levels[j].previewSprite;
                data.scriptableObject = levels[j];
            }
            _stageAnimation.ShowLevels();
        }
    }
}
