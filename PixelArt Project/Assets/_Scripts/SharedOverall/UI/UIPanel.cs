using _Scripts.Gameplay.Shared.UI;
using Assets._Scripts.Menu.Transition;
using UnityEngine;

namespace _Scripts.SharedOverall.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        protected void OnEnable()
        {
            MenuAwaiter.CloseUI += CloseUI;
            StageController.CloseUI += CloseUI;
        }

        protected void OnDisable()
        {
            MenuAwaiter.CloseUI -= CloseUI;
            StageController.CloseUI -= CloseUI;
        }

        public abstract void CloseUI();
    }
}