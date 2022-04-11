using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Gameplay.Recording.DrawingPanel
{
    public class FieldSizeHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Axis thisAxis;
        private TMP_InputField _thisTmpInputField;
        public static int X { get; private set; }
        public static int Y { get; private set; }
        public static int selectedFieldIndex { get; set; }

        private enum Axis
        {
            X, Y
        }
        private void Awake()
        {
            _thisTmpInputField = GetComponent<TMP_InputField>();
            _thisTmpInputField.onValueChanged.AddListener(TextUpdate);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (thisAxis)
            {
                case Axis.X :
                    selectedFieldIndex = 1;
                    break;
                case Axis.Y :
                    selectedFieldIndex = 0;
                    break;
            }
        }
        private void OnDestroy()
        {
            _thisTmpInputField.onValueChanged.RemoveListener(TextUpdate);
        }
        
        private void TextUpdate(string t)
        {
            switch (thisAxis)
            {
              case Axis.X :
                  X = int.Parse(t);
                  break;
              case Axis.Y :
                  Y = int.Parse(t);
                  break;
            }
        }
    }
}
