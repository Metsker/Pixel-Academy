using System;
using GameLogic;
using MapEditor.Recording;
using TMPro;
using UnityEngine;

namespace AnimPlaying
{
    public class AnimUIUpdater : AnimPlaying
    {
        public GameObject createClipUI;
        [SerializeField] private TextMeshProUGUI currentAnimName;
        [SerializeField] private TMP_InputField inputField;
        public static int ClipNumber { get; private set; }

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
            UpdateUIText();
        }

        public void Increase()
        {
            if (ClipNumber < ClipListLoader.AnimationClips.Count-1)
            {
                ClipNumber++;
                UpdateUIText();
            }
            else if (ClipNumber == ClipListLoader.AnimationClips.Count-1)
            {
                ClipNumber++;
                UpdateUIText();
                createClipUI.gameObject.SetActive(true);
                inputField.gameObject.SetActive(true);
            }
        }
        public void Decrease()
        {
            if (ClipNumber <= 0) return;
            ClipNumber--;
            UpdateUIText();
            if(!createClipUI.activeSelf) return;
            createClipUI.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
        }

        private void UpdateUIText()
        {
            currentAnimName.SetText(ClipNumber == ClipListLoader.AnimationClips.Count  ? 
                "Empty" : ClipListLoader.AnimationClips[ClipNumber].name);
        }
    }
}
