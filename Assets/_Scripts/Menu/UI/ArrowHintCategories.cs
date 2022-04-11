using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class ArrowHintCategories : BaseArrowHint
    {
        protected override bool IsShown()
        {
            return PlayerPrefs.GetInt("ArrowHintCategories", 0) == 1;
        }
        protected override void SetShown()
        {
            PlayerPrefs.SetInt("ArrowHintCategories", 1);
        }
    }
}