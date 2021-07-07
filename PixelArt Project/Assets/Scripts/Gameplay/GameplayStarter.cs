using GameLogic;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay
{
    public class GameplayStarter : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image reference;
        [SerializeField] private Button submit;
        [SerializeField] private Animator animator;
        [SerializeField] private Slider progress;
        
        private void Update()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                progress.value = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                animator.enabled = false;
                FindObjectOfType<ClearTool>().Clear();
                FindObjectOfType<PencilTool>().SetColor(Color.black); //Fix
                progress.gameObject.SetActive(false);
                reference.gameObject.SetActive(true);
                submit.gameObject.SetActive(true);
            }
        }
    }
}
