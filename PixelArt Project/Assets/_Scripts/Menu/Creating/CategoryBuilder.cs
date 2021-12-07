using System;
using _Scripts.EditorMod.ScriptableObjectLogic;
using _Scripts.GeneralLogic.Menu.Data;
using _Scripts.GeneralLogic.Menu.Transition;
using _Scripts.GeneralLogic.Menu.UI;
using _Scripts.GeneralLogic.Tools;
using TMPro;
using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.Creating
{
    public class CategoryBuilder : MonoBehaviour
    {
        [SerializeField] protected LevelGroupScriptableObject group;
        [SerializeField] private TextMeshProUGUI label;
        protected CreatingData creatingData;

        private Vector2 _rectStartSize;

        public static event Action<int> FillPool;

        private void Awake()
        {
            creatingData = FindObjectOfType<CreatingData>();
        }

        protected void Start()
        {
            var g = creatingData.categoryInstance;
            var data = g.GetComponent<LevelData>();
            _rectStartSize = data.previewRect.sizeDelta;
        }

        public void OpenCategory()
        {
            if (StageController.IsAnimating || group == null) return;
            
            creatingData.label.text = label.text;
            FillPool?.Invoke(group.levels.Count);
            LoadChildren(creatingData.levelPanel);
        }
        
        protected void LoadChildren(GameObject panel)
        {
            var j = 0;
            foreach (Transform child in panel.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                
                var data = child.GetComponent<LevelData>();
                ImageAdjuster.Adjust(data.previewRect, group.levels[j].previewSprite, _rectStartSize);
                data.preview.sprite = group.levels[j].previewSprite;
                data.scriptableObject = group.levels[j];
                data.groupScriptableObject = group;
                if (DifficultyFilterManager.currentDifficulty != DifficultyFilterManager.Difficulties.None
                    && data.scriptableObject.difficulty != DifficultyFilterManager.currentDifficulty)
                {
                    child.gameObject.SetActive(false);
                }
                
                var isLocked = data.scriptableObject.isLocked;
                var isCompleted = data.scriptableObject.stars > 0;

                switch (isLocked)
                {
                    case true:
                        data.Lock();
                        break;
                    default:
                        data.Reload(isCompleted);
                        break;
                }
                j++;
            }
        }
    }
}
