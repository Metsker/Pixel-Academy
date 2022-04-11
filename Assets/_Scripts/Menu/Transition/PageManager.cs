using System.Collections.Generic;
using System.Linq;
using _Scripts.Menu.UI;
using Assets._Scripts.Menu.Transition;
using UnityEngine;

namespace _Scripts.Menu.Transition
{
    public class PageManager : MonoBehaviour
    {
        public Transform[] dirPos;
        public List<BaseArrowHint> arrows;
        public List<PageButton> PageButtons { get; private set; }
        public static StageController StageController { get; private set; }
        public static Pages CurrentPage { get; set; } = Pages.Main;

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
            PageButtons = FindObjectsOfType<PageButton>().ToList().OrderBy(x => (int)(x.page))
                .ToList();
        }
        private void Start()
        {
            switch (CurrentPage)
            {
                case Pages.Editor:
                    PointToPage(Pages.Editor, 0);
                    break;
                case Pages.Shop:
                    PointToPage(Pages.Shop, 0);
                    break;
            }
        }
        public PageButton GetCurrentPage()
        {
            return PageButtons[(int) CurrentPage];
        }
        public void PointToPage(Pages page, float duration)
        {
            PageButtons[(int)page].Pointer(duration);
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
        public static Pages PreviousPage { get; set; } = Pages.Main;

        private void OnEnable()
        {
            StageController.GoToPreviousPage += GoToPreviousPage;
        }
        private void OnDisable()
        {
            StageController.GoToPreviousPage -= GoToPreviousPage;
        }

        private void GoToPreviousPage()
        {
            switch (PreviousPage != CurrentPage)
            {
                case true:
                    PageButtons[(int)PreviousPage].Pointer(Duration);
                    break;
                case false:
                    Application.Quit();
                    break;
            }
        }
#endif
    }
}