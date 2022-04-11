using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Playing.Resulting;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Gameplay.Playing.Hints
{
    public class OpaqueHint : BaseHint, IPointerClickHandler
    {
        private const float Alpha = 0.25f;
        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            CheckHint();
        }

        public override void TakeHint()
        {
            base.TakeHint();
            var i = 0;
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

        protected override WarningUI.WarningType GetWarningType()
        {
            return WarningUI.WarningType.OpaqueHint;
        }
        protected override int GetWarningPrefs()
        {
            return PlayerPrefs.GetInt("OpaqueHintWarning", 0);
        }
    }
}