using System;
using System.Collections;
using DG.Tweening;
using GeneralLogic;
using GeneralLogic.DrawingPanel;
using GeneralLogic.Tools;
using GeneralLogic.Tools.Instruments;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameplayMod.Creating
{
    public class ReferenceCreator : ReferenceTool, IPointerClickHandler
    {
        [SerializeField] private RectTransform referencePanel;
        [SerializeField] private Image referenceImage;

        private const float FadeDuration = 2f;
        public bool IsCreated { get; private set; }

        
        public new void OnPointerClick(PointerEventData eventData)
        {
            if (GameModManager.CurrentGameMod != GameModManager.GameMod.Play || IsCreated) return;
            base.OnPointerClick(eventData);
            IsCreated = true;
            SetReference(FadeDuration);
        }

        public void SetReference(float duration)
        {
            /*referenceImage.sprite = LevelCreator.scriptableObject.previewSprite;
            ImageAdjuster.Adjust(referencePanel, LevelCreator.scriptableObject.previewSprite);
            referenceImage.DOFade(1, duration);*/
        }
    }
}
