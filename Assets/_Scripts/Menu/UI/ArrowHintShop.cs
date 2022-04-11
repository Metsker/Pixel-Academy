using System.Collections;
using _Scripts.Menu.Transition;
using Assets._Scripts.Menu.Transition;
using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class ArrowHintShop : BaseArrowHint
    {
        protected override bool IsShown()
        {
            return PlayerPrefs.GetInt("ArrowHintShop", 0) == 1;
        }
        protected override void SetShown()
        {
            PlayerPrefs.SetInt("ArrowHintShop", 1);
        }
    }
}