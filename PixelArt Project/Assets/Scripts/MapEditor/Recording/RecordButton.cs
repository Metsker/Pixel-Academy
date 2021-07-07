using System;
using AnimPlaying;
using GameLogic;
using Gameplay;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor.Recording
{
    public class RecordButton : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        private PencilTool _pencil;

        private void Awake()
        {
            _pencil = FindObjectOfType<PencilTool>();
        }

        public void SwitchRecording()
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                Debug.LogWarning("Проигрывается анимация");
                return;
            }

            switch (recorder.isActiveAndEnabled)
            {
                case true :
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                    GetComponent<Image>().color = Color.red;
                    ClickOnPixel.firstSnapshotBool = false;
                    recorder.enabled = false;
                    break;
                case false :
                    if(GameModManager.CurrentGameMod != GameModManager.GameMod.Editor) break;
                    if (AnimUIUpdater.ClipNumber >= ClipListLoader.AnimationClips.Count)
                    {
                        Debug.LogWarning("Create new clip first");
                        break;
                    }
                    _pencil.SetColor(Color.white);
                    recorder.Clip = (AnimationClip) ClipListLoader.AnimationClips[AnimUIUpdater.ClipNumber];
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Recording;
                    GetComponent<Image>().color = Color.green;
                    recorder.enabled = true;
                    break;
            }
        }
    }
}
