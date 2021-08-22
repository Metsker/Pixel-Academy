using TMPro;
using UnityEngine;

namespace EditorMod.DrawingPanel
{
    public class FieldSizeHandler : MonoBehaviour
    {
        [SerializeField] private Axis thisAxis;
        private TMP_InputField _thisTmpInputField;
        public static int X { get; private set; }
        public static int Y { get; private set; }

        private enum Axis
        {
            X, Y
        }
        private void Awake()
        {
            _thisTmpInputField = GetComponent<TMP_InputField>();
            _thisTmpInputField.onValueChanged.AddListener(TextUpdate);
            
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
