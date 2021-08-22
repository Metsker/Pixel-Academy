using System;
using System.Collections;
using EditorMod.DrawingPanel;
using GameplayMod.Creating;
using GeneralLogic.Tools.Logic;
using UnityEngine;

namespace GeneralLogic.DrawingPanel
{
    public class DrawingPanelCreator : MonoBehaviour, ICreator, ICleaner, IBuilder<int>
    {
        [SerializeField] private GameObject pxPreset;
        private FlexibleGridLayout _flexibleGridLayout;
        private DrawingTemplateCreator _drawingTemplateCreator;

        public static int X, Y;
        private int _cashSizeX, _cashSizeY;
        public static event Action ToggleUI;

        private void Awake()
        {
            _drawingTemplateCreator = GetComponent<DrawingTemplateCreator>();
            _flexibleGridLayout = GetComponent<FlexibleGridLayout>();
        }

        public void Create()
        {
            var count = 0;
            switch (GameModManager.CurrentGameMod)
            {
                case GameModManager.GameMod.Play:
                    count = SetGrid(LevelCreator.scriptableObject.xLenght, LevelCreator.scriptableObject.yLenght);
                    break;
                default:
                    count = SetGrid(FieldSizeHandler.X, FieldSizeHandler.Y);
                    break;
            }
            if (count == 0)
            {
                Debug.LogWarning("Размер уже выбран или нулевой");
                return;
            }
            Clear();
            StartCoroutine(Build(count));
        }

        public IEnumerator Build(int count)
        {
            yield return new WaitUntil(() => _flexibleGridLayout.SetSize(true));

            for (var i = 0; i < count; i++)
            {     
                var p = Instantiate(pxPreset, transform);
                p.name = "Px " + "(" + i + ")";
            }
            _drawingTemplateCreator.Create();
            _flexibleGridLayout.SetSize(false);
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play) yield break;
            ToggleUI?.Invoke();
        }

        public void Clear()
        {
            if(!FindObjectOfType<ClickOnPixel>()) return;
            foreach (var pixel in FindObjectsOfType<ClickOnPixel>())
            {
                Destroy(pixel.gameObject);
            }
        }
        
        private int SetGrid(int x, int y)
        {
            if (_cashSizeX == x && _cashSizeY == y || x*y == 0)
            {
                return 0;
            }
            if (GameModManager.CurrentGameMod != GameModManager.GameMod.Play)
            {
                _cashSizeX = x;
                _cashSizeY = y;
                X = x;
                Y = y;
            }
            _flexibleGridLayout.columns = x;
            _flexibleGridLayout.rows = y;
            return x*y;
        }
    }
}
