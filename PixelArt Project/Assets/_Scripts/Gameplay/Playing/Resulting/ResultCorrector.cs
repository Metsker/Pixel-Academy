using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.Saving;
using _Scripts.SharedOverall.UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using static _Scripts.Gameplay.Playing.Creating.LevelCreator;
using static _Scripts.Gameplay.Shared.Tools.Logic.ToolsManager;
using static _Scripts.SharedOverall.DrawingPanel.DrawingTemplateCreator;

namespace _Scripts.Gameplay.Playing.Resulting
{
    public class ResultCorrector : RewardCalculator
    {
        [SerializeField] private SnapshotTaker snapshotTaker;
        [SerializeField] private RectTransform instrumentsPanel;
        
        private Button _button;
        private int _mistakesCount;
        private float _instrumentsY;
        public static bool IsPaused { get; private set; }
        public static Color? SelectedColorCash { get; private set; }
        public static readonly Color32 MistakeColor = Color.red;

        private List<Color32> _colorsToCheck = new();
        private IEnumerator _corrector;
        private IEnumerator _sliderAnimator;
        private IEnumerator _newStageDelay;
        private static float _t;

        private static int _currentStageMistakes;
        private static float _correctionDuration;
        private static bool _isSkipped;
        public static float RecoveryTiming { get; set; } = 0.2f;
        private const float SecUntilNextStage = 2;
        public static event Action<bool> SwitchSkipButton;
        public static event Action ContinueLevel;
        private static event Action StartLevelNow;
        public new static event Action<AudioManager.AudioClickType> PlaySound;
        public static event Action<Vector3, bool> SetHelpDirection;
        public static event Action<bool> SwitchPause;
        
        private new void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            SelectableTool.StartLevel += StartLevel;
            StartLevelNow += StartLevel;
        }

        private void OnDisable()
        {
            SelectableTool.StartLevel -= StartLevel;
            StartLevelNow -= StartLevel;
        }

        private void Start()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) return;
            _button.interactable = false;
            IsPaused = false;
            _instrumentsY = instrumentsPanel.anchoredPosition.y;
            if (SaveData.ClipSliderValue == null) return;
            RecoveryTiming = Mathf.Lerp(ClipSpeedSlider.CorrectionMin,ClipSpeedSlider.CorrectionMax, (float)SaveData.ClipSliderValue);
        }

        public void Submit()
        {
            ClickEvent();
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Play:
                    Compare();
                    break;
                case GameModeManager.GameMode.Paint:
                    snapshotTaker.TakeSnapshot();
                    break;
            }
        }

        private async void Compare()
        {
            SetupComparing();
            await Task.Delay(50);

            if (IsCorrectionNeeded())
            {
                GameStateManager.CurrentGameState = GameStateManager.GameState.Correcting;
                SwitchSkipButton?.Invoke(true);
                _corrector = Corrector();
                StartCoroutine(_corrector);
            }
            else
            {
                CheckStage();
            }
        }
        
        private void SetupComparing()
        {
            IsGameStarted = false;
            _button.interactable = false;
            _colorsToCheck = ImagesList.Select(img => img.color).Select(dummy => (Color32) dummy).ToList();
            
            var selected = ColorPresetSpawner.GetSelected();
            if (selected != null)
            {
                SelectedColorCash = selected.GetImageColor();
            }
            if (Stage == LevelCreator.ScriptableObject.stageScriptableObjects.Count - 1)
            {
                ClipPlaying.SaveState();
            }
        }
        
        private IEnumerator Corrector()
        {
            for (var i = 0; i < ImagesList.Count; i++)
            {
                if (_colorsToCheck[i].Equals((Color32)GetCurrentStageScOb().pixelList[i]) || ImagesList[i] == null) continue;

                CheckTools(i);
                PlaySound?.Invoke(AudioManager.AudioClickType.Click);
                ImagesList[i].color = MistakeColor;
                
                for (float j = 0; j < RecoveryTiming; j+=Time.deltaTime)
                {
                    yield return null;
                    if (!IsPaused) continue;
                    yield return new WaitUntil(() => !IsPaused);
                }

                ImagesList[i].color = GetCurrentStageScOb().pixelList[i];
                
                for (float j = 0; j < RecoveryTiming; j+=Time.deltaTime)
                {
                    yield return null;
                    if (!IsPaused) continue;
                    yield return new WaitUntil(() => !IsPaused);
                }
            }
            SwitchSkipButton?.Invoke(false);
            SetHelpDirection?.Invoke(Vector3.zero, false);
            _newStageDelay = NewStageDelay();
            StartCoroutine(_newStageDelay);
        }

        private void StartLevel()
        {
            if (_newStageDelay == null || !ProgressController.IsComplete()) return;
            StopCoroutine(_newStageDelay);
            CheckStage();
        }
        private IEnumerator SliderAnimator()
        {
            for (_t = 0; _t < 1; _t+=Time.deltaTime/_correctionDuration)
            {
                ProgressController.Slider.value = Mathf.Lerp(0, _correctionDuration, _t);
                yield return null;
                if (!IsPaused) continue;
                yield return new WaitUntil(() => !IsPaused);
            }
        }
        
        private IEnumerator NewStageDelay()
        {
            if (_currentStageMistakes > 0)
            {
                for (float i = 0; i < SecUntilNextStage; i+=Time.deltaTime)
                {
                    yield return null;
                }
            }
            CheckStage();
        }
        
        public void SkipCorrection()
        {
            StopCoroutine(_corrector);
            StopCoroutine(_sliderAnimator);
            ProgressController.Slider.normalizedValue = 1;
            IsPaused = false;
            for (var i = 0; i < ImagesList.Count; i++)
            {
                if (_colorsToCheck[i].Equals((Color32)GetCurrentStageScOb().pixelList[i])) continue;
                ImagesList[i].color = GetCurrentStageScOb().pixelList[i];
            }
            SetHelpDirection?.Invoke(Vector3.zero, false);
            _newStageDelay = NewStageDelay();
            StartCoroutine(_newStageDelay);
        }
        
        public static bool SetCorrectionState(bool state)
        {
            if (ProgressController.IsComplete())
            {
                StartLevelNow?.Invoke();
                return false;
            }
            IsPaused = state;
            SwitchPause?.Invoke(state);
            return true;
        }
        
        private bool IsCorrectionNeeded()
        {
            _currentStageMistakes = 0;
            for (var i = 0; i < ImagesList.Count; i++)
            {
                if (_colorsToCheck[i].Equals((Color32) GetCurrentStageScOb().pixelList[i])) continue;
                _currentStageMistakes++;
                _mistakesCount++;
                PixelList[i].IsWrong = true;
            }
            if (_currentStageMistakes == 0) return false;
            InitCorrectionDuration();
            ProgressController.SetProgressColor(ProgressController.CorrectionColor);
            ProgressController.ToggleSliderState(true);
            _sliderAnimator = SliderAnimator();
            StartCoroutine(_sliderAnimator);
            return true;
        }
        
        public static void InitCorrectionDuration()
        {
            _correctionDuration = _currentStageMistakes * RecoveryTiming * 2;
            ProgressController.Slider.maxValue = _correctionDuration;
            ProgressController.Slider.value = Mathf.Lerp(0, _correctionDuration, _t);
        }
        
        private void CheckStage()
        {
            ProgressController.ToggleSliderState(false);
            if (Stage == LevelCreator.ScriptableObject.stageScriptableObjects.Count - 1)
            {
                SwitchSkipButton?.Invoke(false);
                CalculateStars(_mistakesCount);
            }
            else
            {
                ContinueLevel?.Invoke();
            }
        }

        private void CheckTools(int index)
        {
            var color = ColorPresetSpawner.GetByColor(GetCurrentStageScOb().pixelList[index]);
            if (color != null)
            {
                if (!GetTool(Tools.Pencil).IsSelected())
                {
                    PlaySound?.Invoke(AudioManager.AudioClickType.Tool);
                    GetTool(Tools.Pencil).SelectToolNoStates();
                }
                else if (!color.IsSelected())
                {
                    PlaySound?.Invoke(AudioManager.AudioClickType.Tool);
                    ColorPresetSpawner.GetByColor(GetCurrentStageScOb().pixelList[index]).SelectColor(false);
                    return;
                }
            }
            else if (!GetTool(Tools.Eraser).IsSelected())
            {
                PlaySound?.Invoke(AudioManager.AudioClickType.Tool);
                GetTool(Tools.Eraser).SelectToolNoStates();
            }
            if (!(instrumentsPanel.anchoredPosition.y > _instrumentsY)) return;
            
            var position = instrumentsPanel.anchoredPosition;
            position = new Vector2(position.x, _instrumentsY);
            instrumentsPanel.anchoredPosition = position;
        }
    }
}