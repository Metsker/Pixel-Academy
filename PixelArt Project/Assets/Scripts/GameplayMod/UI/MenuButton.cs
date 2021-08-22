using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayMod.UI
{
    public class MenuButton : MonoBehaviour
    {
        public void GotoMenu() 
        {
            SceneManager.LoadScene(0);
        }
    }
}
