using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic;
using Gameplay;
using MapEditor.Recording;
using UnityEngine;
using UnityEngine.UI;

namespace AnimPlaying
{
    public class AnimPlaying : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected Slider progress;
        protected event Action<string> UpdateUI;
        private List<Color> _previousImageStates;

        protected void OnEnable()
        {
            AnimClipCreator.ChangeClip += OnClipChange;
        }

        protected void OnDisable()
        {
            AnimClipCreator.ChangeClip -= OnClipChange;
        }

        public void OnClipChange()
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = aoc.animationClips.Select(a => 
                new KeyValuePair<AnimationClip, AnimationClip>(a, (AnimationClip) AnimClipLoader.AnimationClips[AnimClipSelector.ClipNumber])).ToList();
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
            UpdateUI?.Invoke(animator.runtimeAnimatorController.animationClips[0].name);
        }
        private void Update()
        {
            if (GameState.CurrentState == GameState.State.AnimPlaying)
            {
                progress.value = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }

        private void SaveState()
        {
            _previousImageStates = new List<Color>();
            foreach (var img in LevelListManager.PixelImagesList)
            {
                _previousImageStates.Add(img.color);
            }
        }

        private void LoadState()
        {
            var i = 0;
            foreach (var img in LevelListManager.PixelImagesList)
            {
                img.color = _previousImageStates[i];
                i++;
            }
        }
        
        public void SwitchAnimate()
        {
            if (GameState.CurrentState == GameState.State.Recording) return;
            switch (animator.isActiveAndEnabled)
            {
                case true :
                    GameState.CurrentState = GameState.State.Gameplay;
                    animator.enabled = false;
                    progress.value = 0;
                    LoadState();
                    break;
                case false :
                    if (AnimClipLoader.AnimationClips.Count == 0)
                    {
                        Debug.LogWarning("Create new clip first");
                        break;
                    }
                    GameState.CurrentState = GameState.State.AnimPlaying;
                    SaveState();
                    animator.Rebind();
                    animator.enabled = true;
                    break;
            }
        }
    }
}
