using System.Threading.Tasks;
using _Scripts.GameplayMod.Animating;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GeneralLogic.DrawingPanel
{
    public class FlexibleGridLayout : LayoutGroup
    {
        private enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
        
        [SerializeField] private RectTransform viewPort;
        [SerializeField] private FitType fitType;

        public int columns;
        public int rows;

        private bool _isBuilding;
        
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2 spacing;
        
        [SerializeField] private bool fitX;
        [SerializeField] private bool fitY;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;

                var sqrRt = Mathf.Sqrt(transform.childCount);

                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }
            
            var rect = rectTransform.rect;
            var parentWidth = rect.width;
            var parentHeight = rect.height;

            var cellWidth = (parentWidth / columns) - ((spacing.x / columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            var cellHeight = (parentHeight / rows) - ((spacing.y / rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);
            
            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            if (cellSize.x > cellSize.y)
            {
                cellSize.x = cellSize.y;
            }
            else if (cellSize.x < cellSize.y)
            {
                cellSize.y = cellSize.x;
            }

            for (var i = 0; i < rectChildren.Count; i ++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }
        

        public async Task SetSize(bool resetSize)
        {
            switch (resetSize)
            {
                case false :
                    CalculateLayoutInputVertical();
                    rectTransform.sizeDelta = new Vector2(cellSize.x * columns, cellSize.y * rows);

                    if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) break;
                    var clipRect = (RectTransform)FindObjectOfType<ClipManager>().pauseImage.transform.parent.transform;
                    clipRect.sizeDelta = rectTransform.sizeDelta;
                    clipRect.position = rectTransform.transform.position;
                    break;
                
                case true :
                    var rect = viewPort.rect;
                    rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                    await Task.Yield();
                    break;
            }
        }
        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }
}