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
            StartCoroutine(ButtonAnim());
        }

        public void StartAnim()
        {
            _drawingPanelCreator.Create();
            GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
            animator.Rebind();
            StopAllCoroutines();
            Invoke(nameof(AnimDelay),1);
            gameObject.SetActive(false);
        }

        private IEnumerator ButtonAnim()
        {
            while (animator.enabled == false)
            {
                gameObject.transform.DOScale(new Vector3(1.15f, 1.15f, 1), 1f/3).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                yield return new WaitForSeconds(100);
            }
        }

        private void AnimDelay()
        {
            //gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            animator.enabled = true;
        }
    }
}
