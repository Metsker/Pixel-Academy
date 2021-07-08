using System;
using System.Collections;
using DG.Tweening;
using GameLogic;
using MapEditor.PresetSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class StartGameAnimation : MonoBehaviour
    {
        private DrawingPanelCreator _drawingPanelCreator;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            _drawingPanelCreator = FindObjectOfType<DrawingPanelCreator>();
        }

        private void Start()
        {
            gameObject.transform.DOScale(new Vector3(1.15f, 1.15f, 1), 1f/3.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void StartAnim()
        {
            _drawingPanelCreator.Create();
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.Rebind();
            animator.enabled = true;
            gameObject.SetActive(false);
            gameObject.transform.DOKill();
        }
    }
}
