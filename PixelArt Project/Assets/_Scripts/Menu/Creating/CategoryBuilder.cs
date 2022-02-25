using System;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Recording.UI;
using _Scripts.Menu.Data;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools;
using Assets._Scripts.Menu.Transition;
using TMPro;
using UnityEngine;

namespace _Scripts.Menu.Creating
{
    public class CategoryBuilder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private LevelGroupsManager.GroupType type;
        private StageController _stageController;
        public static event Action<int> FillPool;

        protected void Awake()
        {
            _stageController = FindObjectOfType<StageController>();
        }

        protected virtual LevelGroupScriptableObject GetGroup()
        {
            return LevelGroupsLoader.levelGroupsLoader.levelGroups[(int)type];
        }
        public void OpenCategory()
        {
            if (StageController.IsAnimating || GetGroup() == null) return;
            
            CreatingData.creatingData.label.text = GetLabel();
            SetLevelPanelParent();
            FillPool?.Invoke(GetGroup().levels.Count);
            LoadChildren(CreatingData.creatingData.levelPanel);
        }
        
        protected void LoadChildren(GameObject panel)
        {
            var j = 0;
            foreach (Transform child in panel.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                
                var data = child.GetComponent<LevelData>();
                ImageAdjuster.Adjust(data.PreviewRect, GetGroup().levels[j].previewSprite, data.RectStartSize);
                data.preview.sprite = GetGroup().levels[j].previewSprite;
                data.ScriptableObject = GetGroup().levels[j];
                if (PageManager.CurrentPage == PageManager.Pages.Main && DifficultyFilterManager.currentDifficulty != DifficultyFilterManager.Difficulties.None
                    && data.ScriptableObject.difficulty != DifficultyFilterManager.currentDifficulty)
                {
                    child.gameObject.SetActive(false);
                }
                var isLocked = data.ScriptableObject.isLocked;

                switch (isLocked)
                {
                    case true:
                        data.Lock();
                        break;
                    default:
                        data.Reload();
                        break;
                }
                j++;
            }
        }

        private void SetLevelPanelParent()
        {
            _stageController.levels.transform.SetParent(PageManager.pages[(int)PageManager.CurrentPage].objToMove.transform);
        }

        protected virtual string GetLabel()
        {
            return label.text;
        }
    }
}
