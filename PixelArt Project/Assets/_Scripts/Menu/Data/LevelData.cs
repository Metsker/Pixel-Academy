using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu.Data
{
    public class LevelData : MonoBehaviour
    {
        public Image preview;
        [HideInInspector]
        public RectTransform previewRect;
        
        public GameObject lockObj;
        public Image shape;
        public Image state;
        public Image[] stars;
        public LevelScriptableObject scriptableObject { get; set; }
        public LevelGroupScriptableObject groupScriptableObject { get; set; }

        private CreatingData _creatingData;

        private void Awake()
        {
            _creatingData = FindObjectOfType<CreatingData>();
            if (preview == null) return;
            previewRect = preview.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if(scriptableObject == null) return;
            if (!scriptableObject.isLocked && lockObj.activeSelf)
            {
                Unlock();
            }
        }

        public void Unlock()
        {
            scriptableObject.isLocked = false;
            shape.sprite = _creatingData.unlockedShape;
            lockObj.SetActive(false);
            state.gameObject.SetActive(false);
            SaveData.SaveLevelData(scriptableObject);
            SaveSystem.SaveDataToFile();
        }

        public void Lock()
        {
            shape.sprite = _creatingData.lockedShape;
            lockObj.SetActive(true);
            state.color = _creatingData.lockedColor;
            state.gameObject.SetActive(true);
            stars[0].transform.parent.gameObject.SetActive(false);
        }

        public void Reload(bool isCompleted)
        {
            shape.sprite = _creatingData.unlockedShape;
            lockObj.SetActive(false);
            state.gameObject.SetActive(isCompleted);
            stars[0].transform.parent.gameObject.SetActive(isCompleted);
            
            
            if (!isCompleted) return;
            state.color = _creatingData.completedColor;
            foreach (var t in stars)       
            {
                if (t.sprite == _creatingData.unfilledStar) continue;
                t.sprite = _creatingData.unfilledStar;
            }
            for (var i = 0; i < scriptableObject.stars; i++)
            {
                stars[i].sprite = _creatingData.filledStar;
            }
        }
    }
}
