using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Instruments;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.UI.Settings;
using _Scripts.SharedOverall.Utility;
using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Playing.Creating
{
    public class LevelCreator : MonoBehaviour, ICreator
    {
        [Header("Resources")]
        [SerializeField] private LevelScriptableObject scriptableObjectInEditor;
        [Header("Dependencies")]
        [SerializeField] private DrawingPanelCreator drawingPanelCreator;
        [SerializeField] private ColorPresetSpawner colorSpawner;
        [SerializeField] private TextMeshProUGUI stageCounter;

        private ClipManager _clipManager;
        private ToolAnimation _stageAnimation;
        public static LevelScriptableObject ScriptableObject { get; set; }
        public static bool IsGameStarted { get; set; }
        public static int Stage { get; private set; }
        
        private Task _taskDelay;
        public static event Action<TextHint.HintType, float> ShowHint;

        private void Awake()
        {
            _clipManager = GetComponent<ClipManager>();
            _stageAnimation = stageCounter.GetComponent<ToolAnimation>();
            IsGameStarted = false;
            if (!GameModeManager.isDebug) return;
            ScriptableObject = scriptableObjectInEditor;
        }

        private void OnEnable()
        {
            ResultCorrector.ContinueLevel += Create;
            LanguageToggler.UpdateUI += UpdateUI;
        }
        private void OnDisable()
        {
            ResultCorrector.ContinueLevel -= Create;
            LanguageToggler.UpdateUI -= UpdateUI;
        }

        private IEnumerator Start()
        {
            Stage = 0;
            _taskDelay = drawingPanelCreator.Create();
            colorSpawner.Create();
            UpdateUI();
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            ShowHint?.Invoke(TextHint.HintType.Watch, 2);
            yield return new WaitWhile(() => TextHint.IsAnimating);
            ClearTool.Clear();
            BuildLevel();
        }
        public void Create()
        {
            Stage++;
            UpdateUI();
            colorSpawner.Create();
            BuildLevel();
        }
        private async void BuildLevel()
        {
            await Task.WhenAll(_taskDelay);
            _stageAnimation.PlayAnimation();
            _clipManager.SetClip();
        }
        private void UpdateUI()
        {
            stageCounter.SetText($"{Dictionaries.GetLocalizedString("Stage")}{Stage+1}/{ScriptableObject.stageScriptableObjects.Count}");
        }
        public static StageScriptableObject GetPreviousStageScOb()
        {
            return ScriptableObject.stageScriptableObjects[Stage-1];
        }
        public static StageScriptableObject GetCurrentStageScOb()
        {
            return ScriptableObject.stageScriptableObjects[Stage];
        }
        public static StageScriptableObject GetLastStageScOb()
        {
            return ScriptableObject.stageScriptableObjects[^1];
        }
    }
}