using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.SharedOverall.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using static _Scripts.Menu.Data.CreatingData;

namespace _Scripts.Menu.Data
{
    public class LevelData : MonoBehaviour
    {
        public Image preview;
        public RectTransform PreviewRect { get; private set; }
        public Vector2 RectStartSize { get; private set; }
        
        public GameObject lockObj;
        public TextMeshProUGUI lockCostTxt;
        public Image shape;
        public Image state;
        public Image[] stars;
        public LevelScriptableObject ScriptableObject { get; set; }

        private void Awake()
        {
            if (preview == null) return;
            PreviewRect = preview.GetComponent<RectTransform>();
            RectStartSize = PreviewRect.sizeDelta;
        }

        private void OnEnable()
        {
            if(ScriptableObject == null) return;
            if (!ScriptableObject.isLocked && lockObj.activeSelf)
            {
                Unlock();
            }
        }

        public void Unlock()
        {
            ScriptableObject.isLocked = false;
            shape.sprite = creatingData.unlockedShape;
            lockObj.SetActive(false);
            state.gameObject.SetActive(false);
            SaveData.SaveLevelData(ScriptableObject);
            SaveSystem.SaveDataToFile();
        }

        public void Lock()
        {
            shape.sprite = creatingData.lockedShape;
            lockCostTxt.text = ScriptableObject.GetCost().ToString();
            lockObj.SetActive(true);
            
            state.color = creatingData.lockedColor;
            state.gameObject.SetActive(true);
            stars[0].transform.parent.gameObject.SetActive(false);
        }

        public void Reload()
        {
            var isCompleted = ScriptableObject.stars > 0;
            shape.sprite = creatingData.unlockedShape;
            lockObj.SetActive(false);
            state.gameObject.SetActive(isCompleted);
            stars[0].transform.parent.gameObject.SetActive(isCompleted);
            
            
            if (!isCompleted) return;
            state.color = creatingData.completedColor;
            foreach (var t in stars)       
            {
                if (t.sprite == creatingData.unfilledStar) continue;
                t.sprite = creatingData.unfilledStar;
            }
            for (var i = 0; i < ScriptableObject.stars; i++)
            {
                stars[i].sprite = creatingData.filledStar;
            }
        }
    }
}
