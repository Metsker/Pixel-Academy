using System;
using System.Collections;
using System.IO;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.DrawingPanel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static _Scripts.Gameplay.Playing.UI.TextHintInvoker;

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    public class SnapshotTaker : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private Button paintingSnapButton;
        [SerializeField] private BorderManager borderManager;
        private Camera _camera;
#if UNITY_EDITOR
        [Header("Recording")]
        [SerializeField] private GameObject recorderUI;
        public static event Action SnapshotCallback;
#endif

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void TakeSnapshot(string filePath)
        {
            StartCoroutine(SnapshotRoutine(filePath));
        }
        
        public void TakeSnapshot()
        {
            const string filePath = "Assets/Resources/lastShot.png";
            StartCoroutine(SnapshotRoutine(filePath));
        }

        private IEnumerator SnapshotRoutine(string filePath)
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Paint)
            {
                paintingSnapButton.interactable = false;
            }

            borderManager.ToggleState(false);
            foreach (var i in DrawingTemplateCreator.PixelList)
            {
                i.ToggleGrid(false);
            }
#if UNITY_EDITOR
            recorderUI.SetActive(false);
#endif
            yield return new WaitForEndOfFrame();
            
            var corners = new Vector3[4];
            targetRect.GetWorldCorners(corners);
            var bl = RectTransformUtility.WorldToScreenPoint(_camera, corners[0]);
            var tl = RectTransformUtility.WorldToScreenPoint(_camera, corners[1]);
            var tr = RectTransformUtility.WorldToScreenPoint(_camera, corners[2]);
            var width = tr.x - bl.x;
            var height = tl.y - bl.y;

            var tex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            Rect rex = new Rect(bl.x,bl.y,width,height);
            tex.ReadPixels(rex, 0, 0);
            tex.Apply();
            borderManager.ToggleState(true);
            foreach (var i in DrawingTemplateCreator.PixelList)
            {
                i.ToggleGrid(true);
            }
#if UNITY_EDITOR
            var bytes = tex.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            AssetDatabase.Refresh();
            recorderUI.SetActive(true);
            SnapshotCallback?.Invoke();
#else
            var shotName = $"{Application.productName}_Drawing_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            NativeGallery.SaveImageToGallery(tex, Application.productName, shotName);
#endif
            Destroy(tex);
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Paint) yield break;
            TextHintInvoker.Invoke(HintType.Snapshot, 3);
            paintingSnapButton.interactable = true;
        }
    }
}