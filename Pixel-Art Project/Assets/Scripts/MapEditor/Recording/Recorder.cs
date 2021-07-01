using System;
using System.Collections.Generic;
using AnimPlaying;
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
        private List<Color> _exportImageStates;
        
        private void OnEnable()
        {
            ClickOnPixel.TakeSnapshot += Snapshot;
            ColorPreset.TakeSnapshot += Snapshot;
            PencilTool.TakeSnapshot += Snapshot;
            EraserTool.TakeSnapshot += Snapshot;
            CreateRecorder();
            Snapshot(SnapshotDelay);
        }
        private void OnDisable()
        {
            ClickOnPixel.TakeSnapshot -= Snapshot;
            ColorPreset.TakeSnapshot -= Snapshot;
            PencilTool.TakeSnapshot -= Snapshot;
            EraserTool.TakeSnapshot -= Snapshot;
            _recorder.SaveToClip(Clip);
            SaveLevelAsset.CreateMyAsset(ExportState());
        }

        private static void Snapshot(float delay)
        {
            _recorder.TakeSnapshot(delay);
        }

        private void CreateRecorder()
        {
            _recorder = new GameObjectRecorder(gameObject);
            _recorder.BindComponentsOfType<RectTransform>(gameObject, true);
            _recorder.BindComponentsOfType<Image>(gameObject, true);
        }

        private List<Color> ExportState()
        {
            _exportImageStates = new List<Color>();
            foreach (var img in LevelListManager.PixelImagesList)
            {
                _exportImageStates.Add(img.color);
            }
            return _exportImageStates;
        }
    }
}
