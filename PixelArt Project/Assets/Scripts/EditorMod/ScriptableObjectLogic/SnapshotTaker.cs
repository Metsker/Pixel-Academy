using System;
using System.Collections;
using System.Globalization;
using System.IO;
using GeneralLogic.DrawingPanel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EditorMod.ScriptableObjectLogic
{
    public class SnapshotTaker : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private Sprite pixelNoGrid;
        public bool isSnaped { get; private set; }
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void TakeSnapshot(string filePath)
        {
            StartCoroutine(SnapshotRoutine(filePath));
        }
        
        public void TakeSnapshot()
        {
            StartCoroutine(SnapshotRoutine("Протестировать снапшот на устройство"));
        }

        private IEnumerator SnapshotRoutine(string filePath)
        {
            var cashSprite = DrawingTemplateCreator.PixelImagesList[0].sprite;
            foreach (var i in DrawingTemplateCreator.PixelImagesList)
            {
                i.sprite = pixelNoGrid;
            }
            
            yield return new WaitForEndOfFrame();
            
            var corners = new Vector3[4];
            targetRect.GetWorldCorners(corners);
            var bl = RectTransformUtility.WorldToScreenPoint(_camera, corners[0]);
            var tl = RectTransformUtility.WorldToScreenPoint(_camera, corners[1]);
            var tr = RectTransformUtility.WorldToScreenPoint(_camera, corners[2]);
            var width = tr.x - bl.x;
            var height = tl.y - bl.y;
            
            Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
            Rect rex = new Rect(bl.x,bl.y,width,height);
            tex.ReadPixels(rex, 0, 0);
            tex.Apply();
            var bytes = tex.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Destroy(tex);
            foreach (var i in DrawingTemplateCreator.PixelImagesList)
            {
                i.sprite = cashSprite;
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
            Debug.Log("Референс сохранен");
#endif
            isSnaped = true;
            yield return new WaitForSeconds(1);
            isSnaped = false;
        }
    }
}