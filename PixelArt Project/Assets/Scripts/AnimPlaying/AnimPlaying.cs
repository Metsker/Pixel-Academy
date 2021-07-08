using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic;
using Gameplay;
using MapEditor.Recording;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace AnimPlaying
{
    public class AnimPlaying : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected Slider progress;
        private List<Color> _previousImageStates;

        private void ChangeClip()
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = aoc.animationClips.Select(a => 
                new KeyValuePair<AnimationClip, AnimationClip>(a, (AnimationClip) ClipListLoader.AnimationClips[AnimUIUpdater.ClipNumber])).ToList();
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

        private void SaveState()
        {
            _previousImageStates = new List<Color>();
            foreach (var img in LevelTemplateCreator.PixelImagesList)
            {
                _previousImageStates.Add(img.color);
            }
        }

        private void LoadState()
        {
            var i = 0;
            foreach (var img in LevelTemplateCreator.PixelImagesList)
            {
                img.color = _previousImageStates[i];
                i++;
            }
        }
        
        public void SwitchAnimate()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording) return;
            switch (animator.isActiveAndEnabled)
            {
                case true :
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                    animator.enabled = false;
                    progress.value = 0;
                    LoadState();
                    FindObjectOfType<PencilTool>().SetColor(GameModManager.GameMod.Editor);
                    break;
                case false :
                    if (ClipListLoader.AnimationClips.Count == 0)
                    {
                        Debug.LogWarning("Create new clip first");
                        break;
                    }
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Animating;
                    SaveState();
                    animator.Rebind();
                    ChangeClip();
                    animator.enabled = true;
                    break;
            }
        }
    }
}
