using TMPro;
using UnityEngine;

namespace MapEditor.PresetSettings
{
    public class DrawingSizeField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField tmpInputField;
        private TMP_InputField _thisTmpInputField;
        public int size;
        
        private void Awake()
        {
            _thisTmpInputField = GetComponent<TMP_InputField>();
            _thisTmpInputField.onValueChanged.AddListener(TextUpdate);
        }
        
        private void TextUpdate(string t)
        {
            if (int.TryParse(t, out size)&&size > 0)
            {
                tmpInputField.text = size.ToString();
            }
            else
            {
                tmpInputField.text = "";
            }
        }
    }
}
