#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.ColorPresets;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace _Scripts.Gameplay.Recording.Recording
{
    public class Recorder : MonoBehaviour
    {
        public const float SnapshotDelay = 0.4f;
        public AnimationClip Clip { get; set; }
        private static GameObjectRecorder RecorderObject { get; set; }
        
        [SerializeField] private GameObject drawingPanel;
        [SerializeField] private GameObject colorPresets;
        [SerializeField] private GameObject instruments;

        [Header("Resources")] 
        [SerializeField] private GameObject uiToSwitch;

        public static Color? SelectedColorCash { get; set; }

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
            RecorderObject.TakeSnapshot(delay);
        }

        public static void Snapshot(AudioManager.AudioClickType audioClickType)
        {
            switch (audioClickType)
            {
                case AudioManager.AudioClickType.Click:
                    _audioClickTimings.Add(RecorderObject.currentTime);
                    break;
                case AudioManager.AudioClickType.Tool:
                    _audioToolTimings.Add(RecorderObject.currentTime);
                    break;
            }
        }

        private void CreateRecorder()
        {
            _audioClickTimings = new List<float>();
            _audioToolTimings = new List<float>();
            RecorderObject = new GameObjectRecorder(gameObject);
            
            foreach (Transform child in drawingPanel.transform)
            {
                RecorderObject.BindComponent(child.GetComponent<Image>());
            }
                
            RecorderObject.BindComponent(colorPresets.transform.GetComponent<RectTransform>());
            foreach (Transform child in colorPresets.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                RecorderObject.BindComponent(child.GetComponent<RectTransform>());
            }
            foreach (var child in colorPresets.GetComponentsInChildren<Transform>())
            {
                if (child.name != "Color") continue;
                RecorderObject.BindComponent(child.GetComponent<Image>());
            }
            
            RecorderObject.BindComponent(instruments.transform.GetComponent<RectTransform>());
            foreach (Transform child in instruments.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                RecorderObject.BindComponent(child.GetComponent<RectTransform>());
            }
            foreach (var child in instruments.GetComponentsInChildren<Transform>())
            {
                if (child.name != "Color") continue;
                RecorderObject.BindComponent(child.GetComponent<Image>());
            }
            
            Snapshot(ColorPresetSpawner.GetSelected() == null ?
                Time.deltaTime : ToolAnimation.AnimDuration);
        }

        public void NextPart()
        {
            if (ColorPresetSpawner.GetSelected() != null)
            {
                SelectedColorCash = ColorPresetSpawner.GetSelected().GetImageColor();
            }
            SaveClip();
            if (!_clipCreator.CreateClip(Clip)) return;
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
            RecorderObject.SaveToClip(Clip);
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
            foreach (var pt in ColorPresetSpawner.ColorPresets)
            {
                pt.AssignStruct();
                _colorPresetList.Add(pt.PresetStruct);
            }
            return _colorPresetList;
        }
    }
}
#endif
