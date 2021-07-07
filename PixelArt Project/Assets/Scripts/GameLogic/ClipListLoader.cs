using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class ClipListLoader : MonoBehaviour
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