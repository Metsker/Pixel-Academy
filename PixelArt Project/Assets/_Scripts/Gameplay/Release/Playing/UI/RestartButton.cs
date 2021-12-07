using _Scripts.GameplayMod.Animating;
using UnityEngine;

namespace _Scripts.GameplayMod.UI
{
    public class RestartButton : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneTransitionManager.OpenScene(1);
        }
    }
}
