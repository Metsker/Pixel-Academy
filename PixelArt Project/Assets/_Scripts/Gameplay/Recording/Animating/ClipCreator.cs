#if (UNITY_EDITOR)
using System;
using System.IO;
using _Scripts.EditorMod.Recording;
using _Scripts.GeneralLogic.DrawingPanel;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _Scripts.EditorMod.Animating
{
    public class ClipCreator : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        private ClipUIUpdater _clipUIUpdater;
        public TMP_InputField inputField;
        private const string ClipPath = "Assets/Resources/Levels";
        public static event Action UpdateUI;

        private void Awake()
        {
            _clipUIUpdater = FindObjectOfType<ClipUIUpdater>();
        }

        private void Start()
        {
            inputField.text = "";
        }

        public void CreateClip()
        {
            if (inputField.text.Length==0)
            {
                Debug.LogWarning("Empty field");
                return;
            }
            var x = DrawingPanelCreator.X;
            var y = DrawingPanelCreator.Y;
            var size = x * y;
            if (size == 0)
            {
                Debug.LogWarning("Создайте поле");
                return;
            }
            var brackets = "(" + x + "x" + y + ")";
            var i = 0;
            while (ClipListLoader.AnimationClips.Find(item => item.name == inputField.text + brackets + "_" + i))
            {
                i++;
            }
            recorder.Clip = new AnimationClip();
            var clipName = inputField.text + brackets;
            if (!AssetDatabase.IsValidFolder($"{ClipPath}/{clipName}"))
            {
                AssetDatabase.CreateFolder(ClipPath, clipName);
                AssetDatabase.CreateFolder($"{ClipPath}/{clipName}", "Data");
            }
            AssetDatabase.CreateAsset(recorder.Clip, $"{ClipPath}/{clipName}/Data/{clipName}_{i}.anim");
            AssetDatabase.SaveAssets();
            ClipListLoader.AnimationClips.Add(recorder.Clip);
            _clipUIUpdater.createClipUI.gameObject.SetActive(false);
            inputField.text = "";
            UpdateUI?.Invoke();
        }

        public void CreateClip(AnimationClip clip)
        {
            
            var partLength = Recorder.Part.ToString().Length;
            var clipName = clip.name.Remove(clip.name.Length - partLength - 1);
            if (File.Exists($"{ClipPath}/{clipName}/Data/{clipName}_{Recorder.Part+1}.anim"))
            {
                Debug.LogWarning("Уже существует");
                return;
            }
            Recorder.Part++;
            recorder.Clip = new AnimationClip();
            AssetDatabase.CreateAsset(recorder.Clip, $"{ClipPath}/{clipName}/Data/{clipName}_{Recorder.Part}.anim");
            AssetDatabase.SaveAssets();
            ClipListLoader.AnimationClips.Add(recorder.Clip);
            ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count - 1;
            UpdateUI?.Invoke();
        }
    }
}
#endif
