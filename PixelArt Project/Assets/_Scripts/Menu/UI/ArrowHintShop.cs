using System;
using System.Collections;
using System.Collections.Generic;
using Assets._Scripts.Menu.Transition;
using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class ArrowHintShop : BaseArrowHint
    {
        private static bool _isShown;
        protected new IEnumerator Start()
        {
            yield return new WaitUntil(() => !PageButton.IsAnimating);
            yield return base.Start();
        }
        protected override bool IsShown()
        {
            return _isShown;
        }
        protected override void SetShown()
        {
            _isShown = true;
        }
    }
}