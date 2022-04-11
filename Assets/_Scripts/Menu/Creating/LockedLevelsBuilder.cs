using System.Collections.Generic;
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

        protected new void Awake()
        {
            base.Awake();
            GetGroup().levels = new List<LevelScriptableObject>();
            GetGroup().levels =
                LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups.
                    Where(g => g.groupType >= 0).
                    SelectMany(group =>
                    group.levels.Where(l => l.isLocked)).OrderBy(l => l.difficulty).
                    ToList();
        }

        protected new void OnEnable()
        {
            base.OnEnable();
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
