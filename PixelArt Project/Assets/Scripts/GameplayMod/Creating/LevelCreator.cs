using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EditorMod.ScriptableObjectLogic;
using GameplayMod.Resulting;
using GeneralLogic;
using GeneralLogic.ColorPresets;
using GeneralLogic.DrawingPanel;
using UnityEngine;

namespace GameplayMod.Creating
{
    public class LevelCreator : MonoBehaviour, ICreator
    {
        [Header("Resources")]
        [SerializeField] private LevelScriptableObject scriptableObjectInEditor;
        public static LevelScriptableObject scriptableObject { get; set; }
        [Header("Dependencies")]
        [SerializeField] private ColorPresetSpawner colorSpawner;
        [SerializeField] private DrawingPanelCreator drawingPanelCreator;
        private GameStateToggler _gameStateToggler;
        public static int Stage { get; private set; }

        private void Awake()
        {
            _gameStateToggler = GetComponent<GameStateToggler>();
            if (!GameModManager.Debug) return;
            scriptableObject = scriptableObjectInEditor;
        }

        private void Start()
        {
            drawingPanelCreator.Create();
            Stage = -1;
            Create();
        }

        public async void Create()
        {
            if (Stage == scriptableObject.stageScriptableObjects.Count - 1 || Stage == scriptableObjectInEditor.stageScriptableObjects.Count)
            {
                return;
            }
            Stage++;
            await Task.Delay(100); //?
            colorSpawner.Create();
            _gameStateToggler.SetClip();
        }
    }
}