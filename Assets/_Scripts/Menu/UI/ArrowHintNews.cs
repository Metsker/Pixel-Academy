using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class ArrowHintNews : BaseArrowHint
    {
        protected override bool IsShown()
        {
            return PlayerPrefs.GetInt("ArrowHintNews", 0) == 1;
        }
        protected override void SetShown()
        {
            PlayerPrefs.SetInt("ArrowHintNews", 1);
        }
    }
}