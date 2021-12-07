using UnityEngine;

namespace _Scripts.Menu.Transition
{
    public class PageManager : MonoBehaviour
    {
        public static Pages currentPage = Pages.Main;
        public PageButton[] pages;
        public const float Duration = 0.7f;
        public static Pages cashPage { get; set; } = Pages.Main;
        
        public enum Pages
        {
            Editor,
            Main,
            Stats
        }

        private void Start()
        {
            switch (currentPage)
            {
                case Pages.Editor:
                    pages[(int)Pages.Editor].Pointer(0);
                    break;
                case Pages.Stats:
                    pages[(int)Pages.Stats].Pointer(0);
                    break;
            }
        }
        
#if UNITY_ANDROID
        public void GotoPreviousPage()
        {
            switch (cashPage != currentPage)
            {
                case true:
                    pages[(int)cashPage].Pointer(Duration);
                    break;
                case false:
                    Application.Quit();
                    break;
            }
        }
#endif
    }
}