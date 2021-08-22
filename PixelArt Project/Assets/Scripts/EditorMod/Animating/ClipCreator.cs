using System;
using EditorMod.Recording;
using GeneralLogic.DrawingPanel;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace EditorMod.Animating
{
    public class ClipCreator : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        private ClipUIUpdater _clipUIUpdater;
        public TMP_InputField inputField;
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
            var x = DrawingPanelCreator.X;
            var y = DrawingPanelCreator.Y;
            var size = x * y;
            if (size == 0)
            {
                Debug.LogWarning("Создайте поле");
                return;
            }
            
            var brackets = "(" + x + "x" + y + ")";
            if (ClipListLoader.AnimationClips.Find(item => item.name == inputField.text + brackets) || inputField.text.Length==0)
            {
                Debug.LogWarning("Name matching or empty field");
                return;
            }
            
            recorder.Clip = new AnimationClip();
            AssetDatabase.CreateAsset(recorder.Clip, "Assets/Resources/Lessons/Clips/" + inputField.text + brackets + "_" + Recorder.Part +".anim");
            AssetDatabase.SaveAssets();
            ClipListLoader.AnimationClips.Add(recorder.Clip);
            _clipUIUpdater.createClipUI.gameObject.SetActive(false);
            inputField.text = "";
            UpdateUI?.Invoke();
        }

        public void CreateClip(AnimationClip clip)
        {
            recorder.Clip = new AnimationClip();
            var partLength = Recorder.Part.ToString().Length;
            AssetDatabase.CreateAsset(recorder.Clip, "Assets/Resources/Lessons/Clips/" + clip.name.Remove(clip.name.Length-partLength) + Recorder.Part +".anim");
            AssetDatabase.SaveAssets();
            ClipListLoader.AnimationClips.Add(recorder.Clip);
            ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count - 1;
            UpdateUI?.Invoke();
        }
    }
}
