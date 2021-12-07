using System.Collections;
using System.Collections.Generic;
using _Scripts.GeneralLogic.ColorPresets;
using TMPro;
using UnityEngine;

public class PasteToPicker : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    public void Paste()
    {
        inputField.onEndEdit.Invoke(GUIUtility.systemCopyBuffer);
    }
}
