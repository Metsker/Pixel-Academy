using TMPro;
using UnityEngine;

namespace _Scripts.SharedOverall.Utility
{
    public class PasteToPicker : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        public void Paste()
        {
            inputField.onEndEdit.Invoke(GUIUtility.systemCopyBuffer);
        }
    }
}
