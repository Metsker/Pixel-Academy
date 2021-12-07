using System.Collections.Generic;
using _Scripts.EditorMod.UI;
using _Scripts.GeneralLogic.Menu.UI;
using UnityEngine;

namespace _Scripts.EditorMod.ScriptableObjectLogic
{
    public class LevelScriptableObject : ScriptableObject
    {
        [Range(0,64)]
        public int xLenght;
        
        [Range(0,64)]
        public int yLenght;
        
        [Range(0,3)]
        public int stars;

        public bool isLocked;

        public LevelGroupManager.GroupType groupType;
        public DifficultyFilterManager.Difficulties difficulty;
        public Sprite previewSprite;
        public List<StageScriptableObject> stageScriptableObjects;
    }
}