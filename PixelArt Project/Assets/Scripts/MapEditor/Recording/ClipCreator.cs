using System;
using AnimPlaying;
using GameLogic;
using MapEditor.PresetSettings;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace MapEditor.Recording
{
    public class ClipCreator : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        public TMP_InputField inputField;
        public static event Action UpdateUI;

        private void Start()
        {
            inputField.text = null;
        }

        public void CreateClip()
        {
            recorder.Clip = new AnimationClip();
            
            if (ClipListLoader.AnimationClips.Find(item => item.name == inputField.text) || inputField.text.Length==0)
            {
                Debug.LogWarning("Name matching or empty field");
                return;
            }
            var size = DrawingPanelCreator.sizeFieldHandler.size;
            var brackets = size != 0 ? "(" + size + "x" + size + ")" : "";
            AssetDatabase.CreateAsset(recorder.Clip, "Assets/Resources/LessonClips/" + inputField.text + brackets + ".anim");
            AssetDatabase.SaveAssets();
            ClipListLoader.AnimationClips.Add(recorder.Clip);
            FindObjectOfType<AnimUIUpdater>().createClipUI.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            inputField.text = null;
            UpdateUI?.Invoke();
        }
    }
}
