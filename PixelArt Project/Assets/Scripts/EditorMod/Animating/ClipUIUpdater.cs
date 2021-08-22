using System;
using EditorMod.DrawingPanel;
using GeneralLogic.Tools.Logic;
using TMPro;
using UnityEngine;

namespace EditorMod.Animating
{
    public class ClipUIUpdater : AnimatorSwitcher
    {
        public GameObject createClipUI;
        private DrawingBuilderUI _drawingBuilderUI;
        [SerializeField] private TextMeshProUGUI currentAnimName;

        private void Awake()
        {
            _drawingBuilderUI = FindObjectOfType<DrawingBuilderUI>();
        }

        private void OnEnable()
        {
            ClipCreator.UpdateUI += UpdateUIText;
        }

        private void OnDisable()
        {
            ClipCreator.UpdateUI -= UpdateUIText;
        }

        private void Start()
        {
            if (ClipListLoader.AnimationClips.Count > 0)
            {
                ChangeClip((AnimationClip) ClipListLoader.AnimationClips[ClipListLoader.ClipNumber]);
                UpdateUIText();
            }
            else
            {
                createClipUI.gameObject.SetActive(true);
            }
            
            _drawingBuilderUI.drawingBuilderUI.SetActive(true);
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
                _drawingBuilderUI.drawingBuilderUI.SetActive(true);
                createClipUI.gameObject.SetActive(true);
            }
        }

        public void GotoLast()
        {
            if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count) return;
            ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count;
            UpdateUIText();
            _drawingBuilderUI.drawingBuilderUI.SetActive(true);
            createClipUI.gameObject.SetActive(true);

        }
        
        public void Decrease()
        {
            if (ClipListLoader.ClipNumber <= 0) return;
            ClipListLoader.ClipNumber--;
            UpdateUIText();
            if (FindObjectOfType<ClickOnPixel>())
            {
                _drawingBuilderUI.drawingBuilderUI.SetActive(false);
            }
            createClipUI.gameObject.SetActive(false);
        }

        private void UpdateUIText()
        {
            currentAnimName.SetText(ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count  ? 
                "Empty" : ClipListLoader.AnimationClips[ClipListLoader.ClipNumber].name);
        }
    }
}
