using AnimPlaying;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor.Recording
{
    public class RecordButton : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        public void SwitchRecording()
        {
            if (GameState.CurrentState == GameState.State.AnimPlaying)
            {
                Debug.LogWarning("Проигрывается анимация");
                return;
            }

            switch (recorder.isActiveAndEnabled)
            {
                case true :
                    GameState.CurrentState = GameState.State.Gameplay;
                    GetComponent<Image>().color = Color.red;
                    recorder.enabled = false;
                    break;
                case false :
                    if (AnimClipSelector.ClipNumber < AnimClipLoader.AnimationClips.Count)
                    {
                        recorder.Clip = (AnimationClip)AnimClipLoader.AnimationClips[AnimClipSelector.ClipNumber];
                        GameState.CurrentState = GameState.State.Recording;
                        GetComponent<Image>().color = Color.green;
                        recorder.enabled = true;
                    }
                    else
                    {
                        Debug.LogWarning("Create new clip first");
                    }
                    break;
            }
        }
    }
}
