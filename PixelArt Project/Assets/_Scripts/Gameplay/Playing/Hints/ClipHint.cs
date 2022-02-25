using System;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.SharedOverall.UI;
using UnityEngine.EventSystems;

namespace _Scripts.Gameplay.Playing.Hints
{
    public class ClipHint : BaseHint, IPointerClickHandler
    {
        public static bool IsHint { get; set; }
        public static event Action RepeatClip;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent();
            CheckHint();
        }
        public override void TakeHint()
        {
            base.TakeHint();
            ClipPlaying.SaveState();
            IsHint = true;
            RepeatClip?.Invoke();
        }

        protected override WarningUI.WarningType GetWarningType()
        {
            return WarningUI.WarningType.ClipHint;
        }
    }
}