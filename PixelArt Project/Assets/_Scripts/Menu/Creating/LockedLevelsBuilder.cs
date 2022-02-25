using System.Linq;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Menu.Data;
using _Scripts.SharedOverall;
using UnityEngine;

namespace _Scripts.Menu.Creating
{
    public class LockedLevelsBuilder : CategoryBuilder
    {
        [SerializeField] private GameObject panel;
        private LevelGroupScriptableObject lockedGroup;

        protected override LevelGroupScriptableObject GetGroup()
        {
            return lockedGroup;
        }

        protected new void Awake()
        {
            base.Awake();
            if(LevelGroupsLoader.levelGroupsLoader == null) return;
        
            lockedGroup = ScriptableObject.CreateInstance<LevelGroupScriptableObject>();
            var lockedLevels =
                LevelGroupsLoader.levelGroupsLoader.levelGroups.SelectMany(group =>
                    group.levels.Where(l => l != null && l.isLocked));
            lockedGroup.levels = lockedLevels.OrderBy(p => p.difficulty).ToList();
        }

        private void OnEnable()
        {
            foreach (Transform child in panel.transform)
            {
                if (child.gameObject.activeSelf) continue;
                child.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            if(GetGroup() == null) return;
            for (var i = 0; i < GetGroup().levels.Count; i++)
            {
                var g = Instantiate(CreatingData.creatingData.categoryInstance, panel.transform);
                g.name = "Locked Level " + i;
            }
            LoadChildren(panel);
        }

        protected override string GetLabel()
        {
            return Dictionaries.GetLocalizedString("LockedLabel");
        }
    }
}
