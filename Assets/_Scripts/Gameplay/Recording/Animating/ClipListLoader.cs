using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Utility;
using UnityEngine;

namespace _Scripts.Gameplay.Recording.Animating
{
    public class ClipListLoader : MonoBehaviour
    {
        public static int ClipNumber { get; set; }
        public static List<AnimationClip> AnimationClips { get; private set; }

        private void Awake()
        {
            StartCoroutine(LoadObjectsList());
        }
        
        private IEnumerator LoadObjectsList()
        {
            yield return new WaitUntil(() => LevelGroupsLoader.LevelGroupsLoaderSingleton != null);
            
            var nc = new NumericComparer();
            AnimationClips = new List<AnimationClip>();
            
            foreach (var group in LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups)
            {
                if (group.groupType < 0) continue;
                
                foreach (var stage in group.levels.SelectMany(level => level.stageScriptableObjects))
                {
                    if (stage == null) continue;
                    AnimationClips.Add(stage.animationClip);
                }
            }
            AnimationClips.Sort((x, y) => nc.Compare(x.name, y.name));
        }

        public static AnimationClip GetCurrentClip()
        {
            return AnimationClips?[ClipNumber];
        }
    }
}
