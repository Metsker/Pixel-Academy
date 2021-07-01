using TMPro;
using UnityEngine;

namespace AnimPlaying
{
    public class AnimClipSelector : AnimPlaying
    {
        public GameObject createClipUI;
        [SerializeField] private TextMeshProUGUI currentAnimName;
        [SerializeField] private TMP_InputField inputField;
        public static int ClipNumber{ get; private set; }

        private new void OnEnable()
        {
            base.OnEnable();
            UpdateUI += SetUIText;
        }

        private new void OnDisable()
        {
            base.OnEnable();
            UpdateUI -= SetUIText;
        }

        public void Increase()
        {
            if (ClipNumber < AnimClipLoader.AnimationClips.Count-1)
            {
                ClipNumber++;
                OnClipChange();
            }
            else if (ClipNumber == AnimClipLoader.AnimationClips.Count-1)
            {
                ClipNumber++;
                SetUIText("Empty");
                createClipUI.gameObject.SetActive(true);
                inputField.gameObject.SetActive(true);
            }
        }
        public void Decrease()
        {
            if (ClipNumber <= 0) return;
            ClipNumber--;
            OnClipChange();
            if(!createClipUI.activeSelf) return;
            createClipUI.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
        }

        private void SetUIText(string nameText)
        {
            currentAnimName.SetText(nameText);
        }
    }
}
