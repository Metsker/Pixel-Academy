#if (UNITY_EDITOR)
using System.Collections.Generic;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.DrawingPanel;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.DrawingPanel;
using _Scripts.SharedOverall.Tools.Instruments;
using _Scripts.SharedOverall.Tools.Logic;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Recording.Recording
{
    public class RecorderSwitcher : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;
        [SerializeField] private List<Button> buttonsToSwitch;
        [SerializeField] private List<Toggle> togglesToSwitch;
        [SerializeField] private List<Image> imagesToSwitch;
        
        private DrawingBuilderUI _drawingBuilderUI;
        private Image _image;

        private void Awake()
        {
            _drawingBuilderUI = FindObjectOfType<DrawingBuilderUI>();
            _image = GetComponent<Image>();
            Recorder.Part = 0;
        }

        public void SwitchRecording()
        {
            switch (recorder.isActiveAndEnabled)
            {
                case true :
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Drawing;
                    recorder.enabled = false;
                    ToggleObjects();
                    ToolsManager.DeselectInstruments();
                    _image.color = Color.red;
                    recorder.EditClip();
                    break;
                case false :
                    #region Conditions
                    if(GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record) break;
                    if (ClipListLoader.ClipNumber >= ClipListLoader.AnimationClips.Count)
                    {
                        Debug.LogWarning("Create new clip first");
                        break;
                    }
                    if (DrawingTemplateCreator.ImagesList == null)
                    {
                        Debug.LogWarning("Создайте сетку");
                        return;
                    }
                    #endregion
                    
                    GameStateManager.CurrentGameState = GameStateManager.GameState.Recording;
                    
                    var currentClipName = ClipListLoader.AnimationClips[ClipListLoader.ClipNumber].name;
                    var index = currentClipName.LastIndexOf('_');
                    var part = currentClipName.Substring(index+1);
                    Recorder.Part = int.Parse(part);
                    if (Recorder.Part == 0)
                    {
                        ClearTool.Clear();
                    }
                    
                    if (Recorder.selectedColorCash == ToolsManager.ColorZero || !ColorPresetSpawner.colorPresets.Exists(c => c.image.color == Recorder.selectedColorCash && ColorPresetSpawner.GetSelected() != null && ColorPresetSpawner.GetSelected().image.color == Recorder.selectedColorCash))
                    {
                        ToolsManager.CurrentTool = ToolsManager.Tools.None;
                        ToolsManager.DeselectTools();
                    }
                    else
                    {
                        var cp = ColorPresetSpawner.GetByColor(Recorder.selectedColorCash);
                        cp.Select();
                        ToolsManager.CurrentTool = ToolsManager.Tools.Pencil;
                        ToolsManager.DeselectInstruments();
                        PencilTool.SetColor(cp.image.color);
                        Recorder.selectedColorCash = ToolsManager.ColorZero;
                    }
                    _drawingBuilderUI.DisablePanel();
                    recorder.Clip = (AnimationClip)ClipListLoader.AnimationClips[ClipListLoader.ClipNumber];
                    _image.color = Color.green;
                    ToggleObjects();
                    recorder.enabled = true;
                    break;
            }
        }
        
        private void ToggleObjects()
        {
            foreach (var b in buttonsToSwitch)
            {
                b.interactable = !b.interactable;
            }
            foreach (var t in togglesToSwitch)
            {
                t.interactable = !t.interactable;
            }
            foreach (var t in imagesToSwitch)
            {
                t.raycastTarget = !t.raycastTarget;
                t.color = t.raycastTarget ? Color.white : new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
#endif