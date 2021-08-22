using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorMod.ScriptableObjectLogic
{
    [CreateAssetMenu]
    public class LevelScriptableObject : ScriptableObject
    {
        public int xLenght;
        public int yLenght;
        public DifficultyCalculator.Difficulties difficulty;
        public Sprite previewSprite;
        public List<StageScriptableObject> stageScriptableObjects;
    }
}