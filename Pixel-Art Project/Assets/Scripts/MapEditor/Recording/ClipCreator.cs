using System;
using MapEditor.PresetSettings;
using MapEditor.Recording;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AnimPlaying
{
    public class AnimClipCreator : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        public TMP_InputField inputField;
        public static event Action ChangeClip;

        private void Start()
        {
            inputField.text = null;
        }

        public void CreateClip()
        {
            recorder.Clip = new AnimationClip();
            
            if (AnimClipLoader.AnimationClips.Find(item => item.name == inputField.text) || inputField.text.Length==0)
            {
                Debug.LogWarning("Name matching or empty field");
                return;
            }

            var s = DrawingPanelCreator.sizeFieldHandler.size;
            AssetDatabase.CreateAsset(recorder.Clip, "Assets/Resources/LessonClips/" + inputField.text + "(" + s + "x" + s + ").anim");
            AssetDatabase.SaveAssets();
            AnimClipLoader.AnimationClips.Add(recorder.Clip);
            FindObjectOfType<AnimClipSelector>().createClipUI.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            inputField.text = null;
            ChangeClip?.Invoke();
        }
    }
}
