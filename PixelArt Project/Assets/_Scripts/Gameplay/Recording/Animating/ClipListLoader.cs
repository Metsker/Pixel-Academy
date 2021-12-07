using System.Collections.Generic;
using System.Linq;
using ns;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.EditorMod.Animating
{
    public class ClipListLoader : MonoBehaviour
    {
        public static int ClipNumber { get; set; }
        public static List<Object> AnimationClips { get; private set; } = new();

        private void Awake()
        {
            LoadObjectsList();
        }
        
        private void LoadObjectsList()
        {
            var nc = new NumericComparer();
            AnimationClips = Resources.LoadAll("Levels/", typeof(AnimationClip)).ToList();
            AnimationClips.Sort((x, y) => nc.Compare(x.name, y.name));
        }
    }
}
