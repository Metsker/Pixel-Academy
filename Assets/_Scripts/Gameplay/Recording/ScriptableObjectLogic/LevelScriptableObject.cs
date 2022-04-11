using System.Collections.Generic;
using _Scripts.Gameplay.Recording.UI;
using UnityEngine;
using static _Scripts.Menu.UI.DifficultyFilterManager;

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    public class LevelScriptableObject : ScriptableObject
    {
        [Range(0,64)]
        public int xLenght;
        
        [Range(0,64)]
        public int yLenght;
        
        [Range(0,5)]
        public int stars;

        public bool isLocked;

        public LevelGroupsManager.GroupType groupType;
        public Difficulties difficulty;
        public Sprite previewSprite;
        public List<StageScriptableObject> stageScriptableObjects;

        private const int ECost = 10, MCost = 20, HCost = 30;

        public int GetCost()
        {
            switch (difficulty)
            {
                case Difficulties.Easy:
                    return ECost;
                case Difficulties.Medium:
                    return MCost;
                case Difficulties.Hard:
                    return HCost;
            }
            return 0;
        }
    }
}