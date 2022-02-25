#if (UNITY_EDITOR)
using System;
using System.Collections;
using _Scripts.Gameplay.Recording.DrawingPanel;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Tools.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace _Scripts.Gameplay.Recording.Animating
{
    public class ClipUIUpdater : AnimatorSwitcher
    {
        public GameObject createClipUI;
        [SerializeField] private GameObject drawingBuilderUI;
        [SerializeField] private TextMeshProUGUI currentAnimName;

        private new void OnEnable()
        {
            base.OnEnable();
            ClipCreator.UpdateUI += UpdateUIText;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            ClipCreator.UpdateUI -= UpdateUIText;
        }

        private IEnumerator Start()
        {
            if (LevelGroupsLoader.levelGroupsLoader.levelGroups.Count > 0)
            {
                yield return new WaitUntil(() => ClipListLoader.GetCurrentClip() != null);
                
                ChangeClip(ClipListLoader.GetCurrentClip());
                UpdateUIText();
            }
            else
            {
                createClipUI.gameObject.SetActive(true);
            }
        }

        public void Increase()
        {
            if (ClipListLoader.ClipNumber < ClipListLoader.AnimationClips.Count-1)
            {
                ClipListLoader.ClipNumber++;
                UpdateUIText();
            }
            else if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count-1)
            {
                ClipListLoader.ClipNumber++;
                UpdateUIText();
                drawingBuilderUI.SetActive(true);
                createClipUI.gameObject.SetActive(true);
            }
        }

        public void GotoLast()
        {
            if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count) return;
            ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count;
            UpdateUIText();
            drawingBuilderUI.SetActive(true);
            createClipUI.gameObject.SetActive(true);
        }
        public void GotoNext()
        {
            if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count) return;
            var n = ClipListLoader.GetCurrentClip().name.Remove(6);
            while (n == ClipListLoader.GetCurrentClip().name.Remove(6))
            {
                if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count - 1)
                {
                    ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count;
                    drawingBuilderUI.SetActive(true);
                    createClipUI.gameObject.SetActive(true);
                    break;
                }
                ClipListLoader.ClipNumber++;
            }
            UpdateUIText();
        }
        public void GotoPrevious()
        {
            if (ClipListLoader.ClipNumber <= 0) return;
            var n = ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count ? 
                "" : ClipListLoader.GetCurrentClip().name.Remove(6);
            do
            {
                if (ClipListLoader.ClipNumber == 0) break;
                ClipListLoader.ClipNumber--;

            } while (!ClipListLoader.GetCurrentClip().name.EndsWith("_0") || n == ClipListLoader.GetCurrentClip().name.Remove(6));
            if (FindObjectOfType<ClickOnPixel>())
            {
                drawingBuilderUI.SetActive(false);
            }
            createClipUI.gameObject.SetActive(false);
            UpdateUIText();
        }

        public void Decrease()
        {
            if (ClipListLoader.ClipNumber <= 0) return;
            ClipListLoader.ClipNumber--;
            UpdateUIText();
            if (FindObjectOfType<ClickOnPixel>())
            {
                drawingBuilderUI.SetActive(false);
            }
            createClipUI.gameObject.SetActive(false);
        }

        public void DeletePart()
        {
            var folderName = ClipListLoader.GetCurrentClip().name
                .Substring(0,ClipListLoader.GetCurrentClip().name.LastIndexOf("_", StringComparison.Ordinal));
            File.Delete($"{LevelAssetSaver.LevelPath}/{folderName}/Data/{ClipListLoader.GetCurrentClip().name}.anim");
            File.Delete($"{LevelAssetSaver.LevelPath}/{folderName}/Data/{ClipListLoader.GetCurrentClip().name}.anim.meta");
            File.Delete($"{LevelAssetSaver.LevelPath}/{folderName}/Data/{ClipListLoader.GetCurrentClip().name}.asset.meta");
            File.Delete($"{LevelAssetSaver.LevelPath}/{folderName}/Data/{ClipListLoader.GetCurrentClip().name}.asset.meta");
            ClipListLoader.AnimationClips.Remove(ClipListLoader.GetCurrentClip());
            Decrease();
        }
        
        private void UpdateUIText()
        {
            currentAnimName.SetText(ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count  ? 
                "Empty" : ClipListLoader.GetCurrentClip().name);
        }
    }
}
#endif