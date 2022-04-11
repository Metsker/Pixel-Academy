using _Scripts.Gameplay.Shared.UI;
using Assets._Scripts.Menu.Transition;
using UnityEngine;

namespace _Scripts.SharedOverall.UI
{
    public abstract class UIPanel : MonoBehaviour
    {

        protected void OnEnable()
        {
#if UNITY_ANDROID
            MenuAwaiter.CloseUI += CloseUI;
            StageController.CloseUI += CloseUI;
#endif
        }
        protected void OnDisable()
        {
#if UNITY_ANDROID
            MenuAwaiter.CloseUI -= CloseUI;
            StageController.CloseUI -= CloseUI;
#endif
        }
        public abstract void CloseUI();
    }
}