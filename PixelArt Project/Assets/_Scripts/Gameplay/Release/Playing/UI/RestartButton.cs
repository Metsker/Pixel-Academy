using _Scripts.Gameplay.Release.Shared.UI;
using UnityEngine;

namespace _Scripts.Gameplay.Release.Playing.UI
{
    public class RestartButton : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneTransitionManager.OpenScene(1);
        }
    }
}
