using System;
using System.Linq;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Recording.UI;
using _Scripts.Menu.Data;
using _Scripts.Menu.Transition;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools;
using _Scripts.SharedOverall.UI.Settings;
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
        private PageManager _pageManager;
        private LevelGroupScriptableObject @group;
        public static event Action<int> FillPool;

        protected void Awake()
        {
            _stageController = FindObjectOfType<StageController>();
            _pageManager = FindObjectOfType<PageManager>();
        }

        protected void OnEnable()
        {
            LanguageToggler.UpdateUI += UpdateLabel;
        }

        private void OnDisable()
        {
            LanguageToggler.UpdateUI -= UpdateLabel;
        }

        protected LevelGroupScriptableObject GetGroup()
        {
            if (@group != null) return @group;
            @group = LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups.Find(g=>g.groupType == type);
            return @group;
        }
        public void OpenCategory()
        {
            if (StageController.IsAnimating || GetGroup() == null) return;

            if (GetType() != typeof(LockedLevelsBuilder))
            {
                LevelGroupsLoader.UpdateGroupOrderByStars(GetGroup());
            }

            UpdateLabel();
            SetLevelPanelParent();
            FillPool?.Invoke(GetGroup().levels.Count);
            LoadChildren(CreatingData.creatingData.levelPanel);
        }

        private void UpdateLabel()
        {
            CreatingData.creatingData.label.SetText(GetLabel());
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
                if (PageManager.CurrentPage == PageManager.Pages.Main && DifficultyFilterManager.CurrentDifficulty != DifficultyFilterManager.Difficulties.None
                    && data.ScriptableObject.difficulty != DifficultyFilterManager.CurrentDifficulty)
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
            _stageController.levels.transform.SetParent(_pageManager.GetCurrentPage().objToMove.transform);
        }

        protected virtual string GetLabel()
        {
            return label.text;
        }
    }
}
