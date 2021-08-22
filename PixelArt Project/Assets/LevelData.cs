using EditorMod.ScriptableObjectLogic;
using GeneralLogic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    public Image preview;
    public RectTransform previewRect;
    public Image state;
    public LevelScriptableObject scriptableObject { get; set; }
}
