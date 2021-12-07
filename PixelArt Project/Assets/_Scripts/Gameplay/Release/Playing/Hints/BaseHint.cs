using System;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.Ads;
using _Scripts.GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GameplayMod.Hints
{
    public abstract class BaseHint : BaseTool
    {
        private Image _image;
        public static event Action UpdateUI;
        public static event Action ShowAd;
        public static event Action<WarningUI.WarningType> ShowWarning;

        private new void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
        }

        protected void CheckHint()
        {
            if (PlayerPrefs.GetInt("HintWarning", 0) == 1 && HaveTokens())
            {
                BuyHint();
            }
            else
            {
                ShowWarning?.Invoke(GetWarningType());
            }
        }

        public void BuyHint()
        {
            PlayerPrefs.SetInt("HintTokens", PlayerPrefs.GetInt("HintTokens",3) - GetCost());
            PlayerPrefs.Save();
            UpdateUI?.Invoke();
            TakeHint();
        }
        public void WatchAd()
        {
            HintAdVideo.HintReference = this;
            ShowAd?.Invoke();
        }
        protected void EnableHint()
        {
            if (_image.raycastTarget) return;
            _image.raycastTarget = true;
            _image.color = Color.white;
        }
        protected void DisableHint()
        {
            _image.raycastTarget = false;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0.5f);
        }
        public abstract void TakeHint();
        public abstract bool HaveTokens();
        public abstract int GetCost();
        protected abstract WarningUI.WarningType GetWarningType();
    }
}