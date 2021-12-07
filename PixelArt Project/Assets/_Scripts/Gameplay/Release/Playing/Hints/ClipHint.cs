﻿using System;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.Animating;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayMod.Hints
{
    public class ClipHint : BaseHint, IPointerClickHandler
    {
        private const int Cost = 1;
        public static bool IsHint { get; set; }
        public static event Action RepeatClip;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            CheckHint();
        }
        public override void TakeHint()
        {
            ClipPlaying.SaveState();
            IsHint = true;
            RepeatClip?.Invoke();
        }
        public override bool HaveTokens()
        {
            return PlayerPrefs.GetInt("HintTokens", 3) >= Cost;
        }
        
        public override int GetCost()
        {
            return Cost;
        }

        protected override WarningUI.WarningType GetWarningType()
        {
            return WarningUI.WarningType.ClipHint;
        }
    }
}