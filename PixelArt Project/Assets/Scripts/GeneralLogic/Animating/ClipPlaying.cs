using System.Collections.Generic;
using System.Linq;
using GeneralLogic.DrawingPanel;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.Animating
{
    public class ClipPlaying : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected Slider progress;
        private List<Color> _cashImageStates;
        public static List<Color> ImageResult;

        protected void ChangeClip(AnimationClip anim)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = aoc.animationClips.Select(a => 
                new KeyValuePair<AnimationClip, AnimationClip>(a, anim)).ToList();
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
        }
        private void Update()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                progress.value = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }
        protected void SaveState()
        {
            _cashImageStates = new List<Color>();
            foreach (var img in DrawingTemplateCreator.PixelImagesList)
            {
                _cashImageStates.Add(img.color);
            }
        }
        public static void SaveResult()
        {
            ImageResult = new List<Color>();
            foreach (var img in DrawingTemplateCreator.PixelImagesList)
            {
                ImageResult.Add(img.color);
            }
        }

        protected void LoadState()
        {
            var i = 0;
            foreach (var img in DrawingTemplateCreator.PixelImagesList)
            {
                img.color = _cashImageStates[i];
                i++;
            }
        }
    }
}
