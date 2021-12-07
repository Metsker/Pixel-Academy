#if (UNITY_EDITOR)
using _Scripts.EditorMod.DrawingPanel;
using _Scripts.GeneralLogic.Tools.Logic;
using TMPro;
using UnityEngine;

namespace _Scripts.EditorMod.Animating
{
    public class ClipUIUpdater : AnimatorSwitcher
    {
        public GameObject createClipUI;
        private DrawingBuilderUI _drawingBuilderUI;
        [SerializeField] private TextMeshProUGUI currentAnimName;

        private new void Awake()
        {
            base.Awake();
            _drawingBuilderUI = FindObjectOfType<DrawingBuilderUI>();
        }

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

        private void Start()
        {
            if (ClipListLoader.AnimationClips.Count > 0)
            {
                ChangeClip((AnimationClip)ClipListLoader.AnimationClips[ClipListLoader.ClipNumber]);
                UpdateUIText();
            }
            else
            {
                createClipUI.gameObject.SetActive(true);
            }
            
            _drawingBuilderUI.drawingBuildUI.SetActive(true);
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
                _drawingBuilderUI.drawingBuildUI.SetActive(true);
                createClipUI.gameObject.SetActive(true);
            }
        }

        public void GotoLast()
        {
            if (ClipListLoader.ClipNumber == ClipListLoader.AnimationClips.Count) return;
            ClipListLoader.ClipNumber = ClipListLoader.AnimationClips.Count;
            UpdateUIText();
            _drawingBuilderUI.drawingBuildUI.SetActive(true);
            createClipUI.gameObject.SetActive(true);
        }

        public void Decrease()
        {
            if (ClipListLoader.ClipNumber <= 0) return;
            ClipListLoader.ClipNumber--;
            UpdateUIText();
            if (FindObjectOfType<ClickOnPixel>())
            {
                _drawingBuilderUI.drawingBuildUI.SetActive(false);
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
#endif