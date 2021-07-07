using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AnimPlaying
{
    public class AnimClipLoader : MonoBehaviour
    {
        public static List<Object> AnimationClips { get; private set; } = new List<Object>();

        private void Awake()
        {
            LoadObjectsList();
        }
        
        private void LoadObjectsList()
        {
            AnimationClips = Resources.LoadAll("LessonClips/", typeof(AnimationClip)).ToList();
        }
    }
}
