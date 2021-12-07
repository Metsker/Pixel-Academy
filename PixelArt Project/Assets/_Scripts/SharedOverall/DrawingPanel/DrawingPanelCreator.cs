using System;
using System.Threading.Tasks;
using _Scripts.Gameplay.Recording.DrawingPanel;
using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.Menu.Logic;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine;

namespace _Scripts.SharedOverall.DrawingPanel
{
    public class DrawingPanelCreator : MonoBehaviour
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

        public async void CreateClick()
        {
            await Create();
        }
        
        public async Task Create()
        {
            var count = 0;
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Play:
                    count = SetGrid(LevelCreator.scriptableObject.xLenght, LevelCreator.scriptableObject.yLenght);
                    break;
                case GameModeManager.GameMode.Paint:
                    count = SetGrid(SizeStep.XSide, SizeStep.YSide);
                    break;
                case GameModeManager.GameMode.Record:
                    count = SetGrid(FieldSizeHandler.X, FieldSizeHandler.Y);
                    break;
            }
            if (count == 0)
            {
                Debug.LogWarning("Размер уже выбран или нулевой");
                return;
            }
            Clear();
            await Task.WhenAll(Build(count));
        }

        private async Task Build(int count)
        {
            await _flexibleGridLayout.SetSize(true);

            for (var i = 0; i < count; i++)
            {     
                var p = Instantiate(pxPreset, transform);
                p.GetComponent<ClickOnPixel>().index = i;
                p.name = "Px " + "(" + i + ")";
            }
            
            await _flexibleGridLayout.SetSize(false);
            _drawingTemplateCreator.Create();
            
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            ToggleUI?.Invoke();
        }

        private void Clear()
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
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play)
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
