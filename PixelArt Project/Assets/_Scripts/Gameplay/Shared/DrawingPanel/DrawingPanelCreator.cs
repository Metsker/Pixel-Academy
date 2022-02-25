using System;
using System.Threading.Tasks;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Recording.DrawingPanel;
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
                    count = SetGrid(LevelCreator.ScriptableObject.xLenght, LevelCreator.ScriptableObject.yLenght);
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
        
#if UNITY_EDITOR
        private async void Update()
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Record
                && Input.GetKeyDown(KeyCode.Return))
            {
                await Create();
            }
        }  
#endif
        private async Task Build(int count)
        {
            await _flexibleGridLayout.SetSize(true);

            for (var i = 0; i < count; i++)
            {     
                var p = Instantiate(pxPreset, transform);
                var click = p.GetComponent<ClickOnPixel>();
                click.Index = i;
                p.name = "Px " + "(" + i + ")";
                if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) continue;
                click.GetImage().color = LevelCreator.GetLastStageScOb().pixelList[i];
            }
            
            await _flexibleGridLayout.SetSize(false);
            _drawingTemplateCreator.Create();
            
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            ToggleUI?.Invoke();
        }

        private void Clear()
        {
            if (DrawingTemplateCreator.PixelList == null || DrawingTemplateCreator.PixelList.Count == 0) return;
            foreach (var pixel in DrawingTemplateCreator.PixelList)
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
