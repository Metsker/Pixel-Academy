using System.Collections.Generic;
using System.Linq;
using _Scripts.Menu.UI;
using UnityEngine;

namespace Assets._Scripts.Menu.Transition
{
    public class PageManager : MonoBehaviour
    {
        public Transform[] dirPos;
        
        public static Pages CurrentPage = Pages.Main;
        public static List<PageButton> pages;
        public List<BaseArrowHint> arrows;
        public static Pages CashPage { get; set; } = Pages.Main;
        public static StageController StageController;
        public const float Duration = 0.7f;

        public enum Direction
        {
            Left,
            Right
        }
        public enum Pages
        {
            Stats,
            Editor,
            Main,
            Shop
        }

        private void Awake()
        {
            StageController = FindObjectOfType<StageController>();
            pages = FindObjectsOfType<PageButton>().ToList().OrderBy(x => (int)(x.page))
                .ToList();
        }
        private void Start()
        {
            switch (CurrentPage)
            {
                case Pages.Editor:
                    pages[(int)Pages.Editor].Pointer(0);
                    break;
                case Pages.Shop:
                    pages[(int)Pages.Shop].Pointer(0);
                    break;
            }
        }

        public Vector2 GetPivot(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return new Vector2(1, 0.5f);
                case Direction.Right:
                    return new Vector2(0, 0.5f);
            }
            return Vector2.zero;
        }
        public Vector3 GetPos(Direction dir)
        {
            return dirPos[(int)dir].position;
        }
#if UNITY_ANDROID
        public void GotoPreviousPage()
        {
            switch (CashPage != CurrentPage)
            {
                case true:
                    pages[(int)CashPage].Pointer(Duration);
                    break;
                case false:
                    Application.Quit();
                    break;
            }
        }
#endif
    }
}