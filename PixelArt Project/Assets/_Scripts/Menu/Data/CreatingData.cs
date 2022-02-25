using _Scripts.SharedOverall.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Menu.Data
{
    public class CreatingData : MonoBehaviour
    {
        public static CreatingData creatingData { get; private set; }
        [Header("Dependencies")]
        public GameObject categoryInstance;
        public GameObject levelPanel;
        public Sprite filledStar;
        public Sprite unfilledStar;
        public Sprite unlockedShape;
        public Sprite lockedShape;
        public Color lockedColor;
        public Color completedColor;
        public TextMeshProUGUI label;

        private void Awake()
        {
            if (creatingData != null && creatingData != this)
            {
                Destroy(gameObject);
            } 
            else 
            {
                creatingData = this;
            }
        }

        private void Start()
        {
            SaveSystem.SetData();
        }
    }
}
