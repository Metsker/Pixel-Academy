using _Scripts.GameplayMod.Creating;
using _Scripts.GameplayMod.Resulting;
using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.DrawingPanel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayMod.Hints
{
    public class OpaqueHint : BaseHint, IPointerClickHandler
    {
        private const int Cost = 2;
        private const float Alpha = 0.25f;

        private void OnEnable()
        {
            ResultCalculator.ContinueLevel += EnableHint;
        }
        private void OnDisable()
        {
            ResultCalculator.ContinueLevel -= EnableHint;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            CheckHint();
        }

        public override void TakeHint()
        {
            var i = 0;
            DisableHint();
            foreach (var img in DrawingTemplateCreator.ImagesList)
            {
                var color = LevelCreator.GetCurrentStageScOb().pixelList[i];
                if (img.color != color)
                {
                    if (color == Color.white)
                    {
                        img.color = Color.white;
                    }
                    else
                    {
                        img.color = new Vector4(color.r, color.g, color.b, Alpha);
                    }
                }
                i++;
            }
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
            return WarningUI.WarningType.OpaqueHint;
        }
    }
}