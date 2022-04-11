using TextSpeech;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class VoiceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    
    private const string LANG_CODE = "en-US";

    private void Start()
    {
        Setup(LANG_CODE);

        CheckPermission();
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        SpeechToText.instance.onBeginningOfSpeechCallback += OnBegin;
    }

    private void CheckPermission()
    {
        Permission.RequestUserPermission(Permission.Microphone);
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone)) return;
        
    }
    
    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    private void OnFinalSpeechResult(string result)
    {
        uiText.SetText(result);
    }

    private void OnPartialSpeechResult(string result)
    {
        uiText.SetText(result);
    }
    private void OnBegin()
    {
        uiText.SetText("start");
    }
    private void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }
}
