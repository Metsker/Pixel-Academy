using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditorMod.Animating;
using EditorMod.ColorPresets;
using EditorMod.ScriptableObjectLogic;
using GeneralLogic.Animating;
using GeneralLogic.ColorPresets;
using GeneralLogic.DrawingPanel;
using GeneralLogic.Tools.Logic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace EditorMod.Recording
{
    public class Recorder : MonoBehaviour
    {
        public const float SnapshotDelay = 0.4f;
        public AnimationClip Clip { get; set; }
        private static GameObjectRecorder recorderObject { get; set; }
        [SerializeField] private GameObject palettePanel;
        [SerializeField] private GameObject drawingPanel;
        [SerializeField] private GameObject fillingTool;
        [Header("Resources")] 
        [SerializeField] private GameObject[] toSwitch;
        private ClipCreator _clipCreator;
        private SnapshotTaker _snapshotTaker;
        private RecorderSwitcher _recorderSwitcher;
        public static int Part { get; private set; }
        
        private List<Color> _pixelList;
        private List<ColorPresetStruct> _colorPresetList;

        private void Awake()
        {
            _snapshotTaker = FindObjectOfType<SnapshotTaker>();
            _recorderSwitcher = FindObjectOfType<RecorderSwitcher>();
            _clipCreator = FindObjectOfType<ClipCreator>();
        }

        private void Start()
        {
            Part = 0;
        }

        private void OnEnable()
        {
            ClickOnPixel.TakeSnapshot += Snapshot;
            ToolAnimation.TakeSnapshot += Snapshot;
            ScrollRecording.TakeSnapshot += Snapshot;
            CreateRecorder();
            foreach (var g in toSwitch)
            {
                g.SetActive(true);
            }
        }
        private void OnDisable()
        {
            ClickOnPixel.TakeSnapshot -= Snapshot;
            ToolAnimation.TakeSnapshot -= Snapshot;
            ScrollRecording.TakeSnapshot -= Snapshot;
            foreach (var g in toSwitch)
            {
                g.SetActive(false);
            }
        }

        private static void Snapshot(float delay)
        {
            recorderObject.TakeSnapshot(delay);
        }

        private void CreateRecorder()
        {
            recorderObject = new GameObjectRecorder(gameObject);
            recorderObject.BindComponentsOfType<Image>(palettePanel, true);
            recorderObject.BindComponentsOfType<RectTransform>(palettePanel, true);
            recorderObject.BindComponentsOfType<Image>(fillingTool, true);
            recorderObject.BindComponentsOfType<RectTransform>(fillingTool, true);
            recorderObject.BindComponentsOfType<Image>(drawingPanel, true);
            Snapshot(SnapshotDelay);
        }

        public void NextPart()
        {
            Part++;
            recorderObject.SaveToClip(Clip);
            LevelAssetSaver.CreateStageAsset(ExportPixels(), ExportColorPresets(), Clip);
            _clipCreator.CreateClip(Clip);
            _recorderSwitcher.SwitchRecording();
        }

        public void ClipCompeted()
        {
            recorderObject.SaveToClip(Clip);
            StartCoroutine(TakeShot());
            _recorderSwitcher.SwitchRecording();
            Part = 0;
        }
        private IEnumerator TakeShot()
        {
            var path = "Assets/Resources/Lessons/Snapshots/" + Clip.name + ".png";
            _snapshotTaker.TakeSnapshot(path);
            yield return new WaitUntil(() => _snapshotTaker.isSnaped);
            LevelAssetSaver.CreateStageAsset(ExportPixels(), ExportColorPresets(), Clip);
        }
        
        private List<Color> ExportPixels()
        {
            _pixelList = new List<Color>();
            foreach (var img in DrawingTemplateCreator.PixelImagesList)
            {
                _pixelList.Add(img.color);
            }
            return _pixelList;
        }
        private List<ColorPresetStruct> ExportColorPresets()
        {
            _colorPresetList = new List<ColorPresetStruct>();
            foreach (var pt in FindObjectsOfType<ColorPreset>().Reverse())
            {
                pt.AssignStruct();
                _colorPresetList.Add(pt.PresetStruct);
            }
            return _colorPresetList;
        }
    }
}
