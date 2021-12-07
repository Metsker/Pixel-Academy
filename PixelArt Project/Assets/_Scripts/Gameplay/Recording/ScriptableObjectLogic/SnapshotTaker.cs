using System;
using System.Collections;
using System.IO;
using _Scripts.GeneralLogic;
using _Scripts.GeneralLogic.DrawingPanel;
using _Scripts.GeneralLogic.Menu.Data;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.EditorMod.ScriptableObjectLogic
{
    public class SnapshotTaker : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private TextMeshProUGUI resultInfoText;
        [SerializeField] private Button paintingSnapButton;
        private Camera _camera;
        
        private const int FadeDuration = 1;
        private const int IntervalDuration = 2;

#if UNITY_EDITOR
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
            const string filePath = "Assets/Resources/lastShot.jpg";
            StartCoroutine(SnapshotRoutine(filePath));
        }

        private IEnumerator SnapshotRoutine(string filePath)
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Paint)
            {
                paintingSnapButton.interactable = false;
            }
            
            foreach (var i in DrawingTemplateCreator.PixelList)
            {
                i.ToggleGrid(false);

            }

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
            foreach (var i in DrawingTemplateCreator.PixelList)
            {
                i.ToggleGrid(true);
            }
#if UNITY_EDITOR
            var bytes = tex.EncodeToJPG();
            File.WriteAllBytes(filePath, bytes);
            AssetDatabase.Refresh();
            SnapshotCallback?.Invoke();
#else
            var shotName = $"{Application.productName}_Drawing_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            NativeGallery.SaveImageToGallery(tex, Application.productName, shotName);
#endif
            Destroy(tex);
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Paint) yield break;
            SetResultInfo();
            paintingSnapButton.interactable = true;
        }

        private void SetResultInfo()
        {
            DOTween.Sequence().Append(resultInfoText.DOFade(1, FadeDuration)).
                AppendInterval(IntervalDuration).Append(resultInfoText.DOFade(0, 1));
        }
    }
}