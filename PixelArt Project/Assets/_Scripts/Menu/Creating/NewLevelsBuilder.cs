using System;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Menu.Data;
using _Scripts.SharedOverall;
using UnityEngine;

namespace _Scripts.Menu.Creating
{
    public class NewLevelsBuilder : CategoryBuilder
    {
        [SerializeField] private GameObject panel;

        protected override LevelGroupScriptableObject GetGroup()
        {
            return LevelGroupsLoader.levelGroupsLoader == null ? null : LevelGroupsLoader.levelGroupsLoader.newsLevelGroup;
        }

        private void Start()
        {
            if (GetGroup() == null) return;
            for (var i = 0; i < GetGroup().levels.Count; i++)
            {
                var g = Instantiate(CreatingData.creatingData.categoryInstance, panel.transform);
                g.name = "New Level " + i;
            }
            LoadChildren(panel);
        }
    }
}