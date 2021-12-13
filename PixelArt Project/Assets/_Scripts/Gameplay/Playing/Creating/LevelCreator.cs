using System.Collections;
using System.Threading.Tasks;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Release.Playing.Animating;
using _Scripts.Gameplay.Release.Playing.Resulting;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace _Scripts.Gameplay.Release.Playing.Creating
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
        private ToolAnimation _toolAnimation;
        public static LevelScriptableObject scriptableObject { get; set; }
        public static LevelGroupScriptableObject groupScriptableObject { get; set; }
        public static bool isGameStarted { get; set; }
        public static int Stage { get; private set; }
        private Task _taskDelay;

        private void Awake()
        {
            _clipManager = GetComponent<ClipManager>();
            _toolAnimation = stageCounter.GetComponent<ToolAnimation>();
            if (!GameModeManager.isDebug) return;
            scriptableObject = scriptableObjectInEditor;
        }

        private void OnEnable()
        {
            ResultCalculator.ContinueLevel += Create;
        }
        private void OnDisable()
        {
            ResultCalculator.ContinueLevel -= Create;
        }

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            Stage = 0;
            _taskDelay = drawingPanelCreator.Create();
            colorSpawner.Create();
            UpdateUI();
            stageCounter.alpha = 1;
            yield return new WaitUntil(SceneTransitionManager.IsLoaded);
            BuildLevel();
        }

        
        public static StageScriptableObject GetPreviousStageScOb()
        {
            return scriptableObject.stageScriptableObjects[Stage-1];
        }
        public static StageScriptableObject GetCurrentStageScOb()
        {
            return scriptableObject.stageScriptableObjects[Stage];
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
            _toolAnimation.PlayAnimation();
            _clipManager.SetClip();
        }
        private void UpdateUI()
        {
            stageCounter.SetText($"{Stage+1}/{scriptableObject.stageScriptableObjects.Count}");
        }
    }
}