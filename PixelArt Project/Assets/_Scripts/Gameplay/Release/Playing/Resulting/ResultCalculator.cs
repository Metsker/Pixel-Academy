using System;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.UI;
using static _Scripts.Gameplay.Release.Playing.Creating.LevelCreator;
using static _Scripts.SharedOverall.DrawingPanel.DrawingTemplateCreator;

namespace _Scripts.Gameplay.Release.Playing.Resulting
{
    public class ResultCalculator : RewardCalculator
    {
        [SerializeField] private SnapshotTaker snapshotTaker;
        private Button _button;
        private EraserTool _eraserTool;
        private bool _lastStageMistake;
        private int _mistakesCount;
        public static bool Skip { get; set; }
        public static Color selectedColorCash;
        public static event Action<bool> SwitchSkipButton;
        public static event Action ContinueLevel;
        public new static event Action<AudioClick.AudioClickType> PlaySound;
        public static event Action<Vector3, bool> SetHelpDirection;
        
        private new void Awake()
        {
            base.Awake();
            _button = GetComponentInParent<Button>();
            _eraserTool = FindObjectOfType<EraserTool>();
        }

        private void Start()
        {
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) return;
            _button.interactable = false;
            Skip = false;
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

        private void SetupComparing()
        {
            ToolsManager.CurrentTool = ToolsManager.Tools.None;
            
            var selected = ColorPresetSpawner.GetSelected();
            
            if (selected != null)
            {
                selectedColorCash = selected.image.color;
            }
            if (Stage == scriptableObject.stageScriptableObjects.Count - 1)
            {
                ClipPlaying.SaveState();
            }
        }
        private async void Compare()
        {
            isGameStarted = false;
            SetupComparing();
            _button.interactable = false;
            SwitchSkipButton?.Invoke(true);
            var colorsToCheck = ImagesList.Select(img => img.color).Select(dummy => (Color32) dummy).ToList();

            for (var i = 0; i < ImagesList.Count; i++)
            {
                if (colorsToCheck[i].Equals((Color32)GetCurrentStageScOb().pixelList[i])) continue;
                _mistakesCount++;
                if (Stage == scriptableObject.stageScriptableObjects.Count - 1 && !_lastStageMistake)
                {
                    _lastStageMistake = true;
                }
                if (ImagesList[i] == null || Skip) continue;
                CheckTools(i);
                PlaySound?.Invoke(AudioClick.AudioClickType.Click);
                ImagesList[i].color = Color.red;
                await Task.Delay(200);
                if (ImagesList[i] == null || Skip) continue;
                ImagesList[i].color = GetCurrentStageScOb().pixelList[i];
                await Task.Delay(200);
            }
            CheckStage();
            SetHelpDirection?.Invoke(Vector3.zero, false);
        }
        
        private void CheckStage()
        {
            if (Stage == scriptableObject.stageScriptableObjects.Count - 1)
            {
                SwitchSkipButton?.Invoke(false);
                CalculateStars(_mistakesCount, _lastStageMistake);
            }
            else
            {
                ContinueLevel?.Invoke();
            }
        }

        private void CheckTools(int index)
        {
            if (ColorPresetSpawner.GetByColor(GetCurrentStageScOb().pixelList[index]) != null)
            {
                if (ColorPresetSpawner.GetByColor(GetCurrentStageScOb().pixelList[index]).IsSelected()) return;
                PlaySound?.Invoke(AudioClick.AudioClickType.Tool);
                ColorPresetSpawner.GetByColor(GetCurrentStageScOb().pixelList[index]).SelectWithAnimation();
            }
            else
            {
                if (_eraserTool.IsSelected()) return;
                PlaySound?.Invoke(AudioClick.AudioClickType.Tool);
                _eraserTool.SelectWithAnimation();
            }
        }
    }
}