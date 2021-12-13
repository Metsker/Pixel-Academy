using _Scripts.SharedOverall;
using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Playing.Hints
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hintCount;

        private void OnEnable()
        {
            BaseHint.UpdateUI += UpdateHintUI;
#if UNITY_EDITOR
            Developer.UpdateUI += UpdateHintUI;
#endif
        }
        private void OnDisable()
        {
            BaseHint.UpdateUI -= UpdateHintUI;
#if UNITY_EDITOR
            Developer.UpdateUI -= UpdateHintUI;
#endif
        }
        private void Start()
        {
            UpdateHintUI();
        }
        private void UpdateHintUI()
        {
            hintCount.SetText(PlayerPrefs.GetInt("HintTokens",3).ToString());
        }
    }
}