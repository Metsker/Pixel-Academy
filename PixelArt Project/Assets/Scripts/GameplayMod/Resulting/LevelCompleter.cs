using GameplayMod.Creating;
using GeneralLogic.Animating;
using GeneralLogic.DrawingPanel;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayMod.Resulting
{
    public class LevelCompleter : MonoBehaviour
    {
        [SerializeField] private GameObject completeUI;
        [SerializeField] private GameObject blur;
        [Header("Drawing result")]
        [SerializeField] private FlexibleGridLayout resultGrid;
        [SerializeField] private FlexibleGridLayout drawingGrid;
        [SerializeField] private GameObject pxPrefab;

        public void Complete()
        {
            completeUI.SetActive(true);
            blur.SetActive(true);
            BuildPixels("Result");
            GameStateToggler.isGameStarted = false;
            /*if (_referenceCreator.IsCreated) return;
            _referenceCreator.SetReference(0);*/
        }
        
        private void BuildPixels(string pxName)
        {
            resultGrid.columns = drawingGrid.columns;
            for (var i = 0; i < DrawingTemplateCreator.PixelImagesList.Count; i++)
            {
                var obj = Instantiate(pxPrefab, resultGrid.transform);
                obj.name = "Px " + "(" + i + ")" + " " + pxName;
                var image = obj.GetComponent<Image>();
                image.color = ClipPlaying.ImageResult[i];
            }
            resultGrid.SetSize(false);
        }
    }
}