#if (UNITY_EDITOR)
using System.Collections.Generic;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Recording.Recording
{
    public class Recorder : MonoBehaviour
    {
        public const float SnapshotDelay = 0.4f;
        public AnimationClip Clip { get; set; }
        private static GameObjectRecorder recorderObject { get; set; }
        
        [SerializeField] private GameObject drawingPanel;
        [SerializeField] private GameObject palettePanel;
        [SerializeField] private GameObject fillingTool;
        
        [Header("Resources")] 
        [SerializeField] private GameObject uiToSwitch;

        public static Color selectedColorCash;
        
        private ClipCreator _clipCreator;
        private SnapshotTaker _snapshotTaker;
        private RecorderSwitcher _recorderSwitcher;
        private List<Color> _pixelList;
        private List<ColorPresetStruct> _colorPresetList;
        private static List<float> _audioClickTimings;
        private static List<float> _audioToolTimings;
        public static int Part { get; set; }
        
        private void Awake()
        {
            _snapshotTaker = FindObjectOfType<SnapshotTaker>();
            _recorderSwitcher = FindObjectOfType<RecorderSwitcher>();
            _clipCreator = FindObjectOfType<ClipCreator>();
            GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
        }

        private void OnEnable()
        {
            SnapshotTaker.SnapshotCallback += CreateAsset;
            CreateRecorder();
            uiToSwitch.SetActive(true);
        }
        private void OnDisable()
        {
            SnapshotTaker.SnapshotCallback -= CreateAsset;
            uiToSwitch.SetActive(false);
        }
        
        public static void Snapshot(float delay)
        {
            recorderObject.TakeSnapshot(delay);
        }

        public static void Snapshot(AudioClick.AudioClickType audioClickType)
        {
            switch (audioClickType)
            {
                case AudioClick.AudioClickType.Click:
                    _audioClickTimings.Add(recorderObject.currentTime);
                    break;
                case AudioClick.AudioClickType.Tool:
                    _audioToolTimings.Add(recorderObject.currentTime);
                    break;
            }
        }

        private void CreateRecorder()
        {
            _audioClickTimings = new List<float>();
            _audioToolTimings = new List<float>();
            recorderObject = new GameObjectRecorder(gameObject);
            
            foreach (Transform child in drawingPanel.transform)
            {
                recorderObject.BindComponent(child.GetComponent<Image>());
            }
            recorderObject.BindComponentsOfType<Image>(palettePanel, true);
            recorderObject.BindComponentsOfType<RectTransform>(palettePanel, true);
            recorderObject.BindComponentsOfType<Image>(fillingTool, true);
            recorderObject.BindComponentsOfType<RectTransform>(fillingTool, true);
            Snapshot(Time.deltaTime);
            if (ColorPresetSpawner.GetSelected() != null)
            {
                Snapshot(ToolAnimation.AnimDuration);
            }
        }

        public void NextPart()
        {
            var selected = ColorPresetSpawner.GetSelected();
            if (selected != null)
            {
                selectedColorCash = selected.image.color;
            }
            SaveClip();
            _clipCreator.CreateClip(Clip);
            _recorderSwitcher.SwitchRecording();
        }

        public void EditClip()
        {
            SaveClip();
        }
        
        public void CompeteClip()
        {
            TakeShot();
        }

        private void CreateAsset()
        {
            SaveClip();
            LevelAssetSaver.CreateLevelAsset();
            _recorderSwitcher.SwitchRecording();
        }

        private void SaveClip()
        {
            recorderObject.SaveToClip(Clip);
            LevelAssetSaver.CreateStageAsset(ExportPixels(), ExportColorPresets(),
                Clip, _audioClickTimings, _audioToolTimings);
        }
        
        private void TakeShot()
        {
            var partLength = Part.ToString().Length;
            var shotName = Clip.name.Remove(Clip.name.Length - partLength - 1);
            var path = $"Assets/Resources/Levels/{shotName}/{shotName}.jpg";
            _snapshotTaker.TakeSnapshot(path);
        }
        
        private List<Color> ExportPixels()
        {
            _pixelList = new List<Color>();
            foreach (var img in DrawingTemplateCreator.ImagesList)
            {
                _pixelList.Add(img.color);
            }
            return _pixelList;
        }
        private List<ColorPresetStruct> ExportColorPresets()
        {
            _colorPresetList = new List<ColorPresetStruct>();
            foreach (var pt in ColorPresetSpawner.colorPresets)
            {
                pt.AssignStruct();
                _colorPresetList.Add(pt.PresetStruct);
            }
            return _colorPresetList;
        }
    }
}
#endif
