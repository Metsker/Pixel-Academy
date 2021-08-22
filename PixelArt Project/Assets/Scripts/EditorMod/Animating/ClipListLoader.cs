using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorMod.Animating
{
    public class ClipListLoader : MonoBehaviour
    {
        public static int ClipNumber { get; set; }
        public static List<Object> AnimationClips { get; private set; } = new List<Object>();

        private void Awake()
        {
            LoadObjectsList();
        }
        
        private void LoadObjectsList()
        {
            AnimationClips = Resources.LoadAll("Lessons/Clips/", typeof(AnimationClip)).ToList();
        }
    }
}
