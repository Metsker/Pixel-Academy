using System;
using System.Collections.Generic;
using AnimPlaying;
using GameLogic;
using Gameplay;
using MapEditor.ColorPresets;
using Tools;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor.Recording
{
    public class Recorder : MonoBehaviour
    {
        public const float SnapshotDelay = 0.4f;
        public AnimationClip Clip { get; set; }
        private static GameObjectRecorder _recorder;
        [SerializeField] private GameObject colorPresetsPanel;
        [SerializeField] private GameObject drawingPanel;
        [SerializeField] private GameObject toolsPanel;
        
        private List<Color> _pixelList;
        private List<ColorPresetStruct> _colorPresetList;
        
        private void OnEnable()
        {
            ClickOnPixel.TakeSnapshot += Snapshot;
            ColorPreset.TakeSnapshot += Snapshot;
            PencilTool.TakeSnapshot += Snapshot;
            EraserTool.TakeSnapshot += Snapshot;
            ColorPresetInstanceButton.UpdateRecorder += CreateColorPresetRecorder;
            CreateRecorder();
            Snapshot(SnapshotDelay);
        }
        private void OnDisable()
        {
            ClickOnPixel.TakeSnapshot -= Snapshot;
            ColorPreset.TakeSnapshot -= Snapshot;
            PencilTool.TakeSnapshot -= Snapshot;
            EraserTool.TakeSnapshot -= Snapshot;
            ColorPresetInstanceButton.UpdateRecorder -= CreateColorPresetRecorder;
            ResetPresetColors();
            _recorder.SaveToClip(Clip);
            LevelAssetSaver.CreateMyAsset(ExportPixels(), ExportColorPresets());
        }

        private static void Snapshot(float delay)
        {
            _recorder.TakeSnapshot(delay);
        }

        private void CreateRecorder()
        {
            _recorder = new GameObjectRecorder(gameObject);
            _recorder.BindComponentsOfType<Image>(drawingPanel, true);
            _recorder.BindComponentsOfType<Image>(toolsPanel, true);
            CreateColorPresetRecorder();
        }

        private void CreateColorPresetRecorder()
        {
            _recorder.BindComponentsOfType<RectTransform>(colorPresetsPanel, true);
        }

        private List<Color> ExportPixels()
        {
            _pixelList = new List<Color>();
            foreach (var img in LevelTemplateCreator.PixelImagesList)
            {
                _pixelList.Add(img.color);
            }
            return _pixelList;
        }
        private List<ColorPresetStruct> ExportColorPresets()
        {
            _colorPresetList = new List<ColorPresetStruct>();
            foreach (var pt in FindObjectsOfType<ColorPreset>())
            {
                pt.AssignStruct();
                _colorPresetList.Add(pt.PresetStruct);
            }
            return _colorPresetList;
        }

        private void ResetPresetColors()
        {
            foreach (var ps in FindObjectsOfType<ColorPreset>())
            {
                ps.changed = false;
            }
        }
    }
}
