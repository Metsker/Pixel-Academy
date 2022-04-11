using System;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Ads;
using _Scripts.SharedOverall.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.Hints
{
    public abstract class BaseHint : BaseTool
    {
        private Image _image;
        public static event Action ShowAd;
        public static event Action<WarningUI.WarningType> ShowWarning;

        private new void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
        }

        protected void CheckHint()
        {
            if (GetWarningPrefs() == 1)
            {
                GetHintByAd();
            }
            else
            {
                ShowWarning?.Invoke(GetWarningType());
            }
        }
        public void GetHintByAd()
        {
            HintRewardedAdVideo.HintReference = this;
            ShowAd?.Invoke();
        }
        protected void EnableHint()
        {
            if (_image.raycastTarget) return;
            _image.raycastTarget = true;
            _image.color = Color.white;
        }

        private void DisableHint()
        {
            _image.raycastTarget = false;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0.5f);
        }
        public virtual void TakeHint()
        {
            DisableHint();
        }
        protected override void BaseStates()
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play &&
                GameStateManager.CurrentGameState != GameStateManager.GameState.Drawing)
            {
                TextHintInvoker.Invoke(TextHintInvoker.HintType.BaseStates, 2);
                throw new Exception("Game isn't started");
            }
            base.BaseStates();
        }
        protected abstract WarningUI.WarningType GetWarningType();
        protected abstract int GetWarningPrefs();
    }
}