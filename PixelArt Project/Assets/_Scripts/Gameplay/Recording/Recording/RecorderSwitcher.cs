#if (UNITY_EDITOR)
using System.Collections.Generic;
using _Scripts.Gameplay.Recording.Animating;
using _Scripts.Gameplay.Recording.DrawingPanel;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Instruments;
using _Scripts.Gameplay.Shared.Tools.Logic;
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
                    
                    var currentClipName = ClipListLoader.GetCurrentClip().name;
                    var index = currentClipName.LastIndexOf('_');
                    var part = currentClipName.Substring(index+1);
                    Recorder.Part = int.Parse(part);
                    if (Recorder.Part == 0)
                    {
                        ClearTool.Clear();
                        ToolsManager.CurrentTool = ToolsManager.Tools.None;
                        ToolsManager.DeselectTools();
                        ToolsManager.DeselectColors();
                        Recorder.SelectedColorCash = null;
                    }
                    else if (ToolsManager.CurrentTool == ToolsManager.Tools.None || ToolsManager.CurrentTool == ToolsManager.Tools.Eraser)
                    {
                        ToolsManager.SelectTool(ToolsManager.Tools.Pencil);
                    }
                    
                    if (Recorder.SelectedColorCash != null && ColorPresetSpawner.GetByColor(Recorder.SelectedColorCash.Value) != null)
                    {
                        var cp = ColorPresetSpawner.GetByColor(Recorder.SelectedColorCash.Value);
                        cp.SelectWithoutAnimation();
                        ColorPreset.SetColor(cp.GetImageColor());
                    }
                    else
                    {
                        ToolsManager.DeselectColors();
                    }
                    _drawingBuilderUI.DisablePanel();
                    recorder.Clip = ClipListLoader.GetCurrentClip();
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
            }
        }
    }
}
#endif