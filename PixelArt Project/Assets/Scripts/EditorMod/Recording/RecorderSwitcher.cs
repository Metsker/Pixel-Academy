using System;
using System.Collections.Generic;
using EditorMod.Animating;
using EditorMod.DrawingPanel;
using GeneralLogic;
using GeneralLogic.DrawingPanel;
using GeneralLogic.Tools;
using GeneralLogic.Tools.Instruments;
using GeneralLogic.Tools.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace EditorMod.Recording
{
    public class RecorderSwitcher : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        [SerializeField] private List<Button> buttonsToSwitch;
        private ClearTool _clearTool;
        private DrawingBuilderUI _drawingBuilderUI;
        private Image _image;

        private void Awake()
        {
            _clearTool = FindObjectOfType<ClearTool>();
            _drawingBuilderUI = FindObjectOfType<DrawingBuilderUI>();
            _image = GetComponent<Image>();
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
                    recorder.enabled = false;
                    foreach (var b in buttonsToSwitch)
                    {
                        b.interactable = true;
                    }
                    _image.color = Color.red;
                    ToolsManager.ResetActiveTool(true);
                    break;
                case false :
                    if(GameModManager.CurrentGameMod != GameModManager.GameMod.Record) break;
                    if (ClipListLoader.ClipNumber >= ClipListLoader.AnimationClips.Count)
                    {
                        Debug.LogWarning("Create new clip first");
                        break;
                    }
                    if (DrawingTemplateCreator.PixelImagesList == null)
                    {
                        Debug.LogWarning("Создайте сетку");
                        return;
                    }
                    if (Recorder.Part == 0)
                    {
                        _clearTool.Clear();
                    }
                    ToolsManager.ResetActiveTool(true);
                    _drawingBuilderUI.DisablePanel();
                    recorder.Clip = (AnimationClip)ClipListLoader.AnimationClips[ClipListLoader.ClipNumber];
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Recording;
                    _image.color = Color.green;
                    foreach (var b in buttonsToSwitch)
                    {
                        b.interactable = false;
                    }
                    recorder.enabled = true;
                    break;
            }
        }
    }
}
