using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Shared.ColorPresets;
using _Scripts.Gameplay.Shared.Tools.Instruments;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Tools.Instruments;
using _Scripts.SharedOverall.Tools.Palette;
using UnityEngine;

namespace _Scripts.Gameplay.Shared.Tools.Logic
{
    public class ToolsManager : MonoBehaviour
    {
        public static Tools CurrentTool { get; set; } = Tools.None;
        private static PencilTool Pencil { get; set; }
        private static FillingTool Filler { get; set; }
        private static PipetteTool Pipette { get; set; }
        private static EraserTool Eraser { get; set; }
        
        private static List<ISelectable> _toolsList;

        private void Awake()
        {
            Pencil = FindObjectOfType<PencilTool>();
            Filler = FindObjectOfType<FillingTool>();
            Pipette = FindObjectOfType<PipetteTool>();
            Eraser = FindObjectOfType<EraserTool>();

            _toolsList = new List<ISelectable>
            {
                Pencil,Filler,Pipette,Eraser
            };
            _toolsList.RemoveAll(selectable => selectable == null);
        }

        public enum Tools
        {
            None,
            Pencil,
            Filler,
            Pipette,
            Eraser
        }
        
        public static void DeselectTools()
        {
            foreach (var t in _toolsList.Where(t => t.IsSelected()))
            {
                t.Deselect();
            }
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play || CurrentTool == Tools.Pencil) return;
            PickerHandler.DisablePicker();
        }
        public static void DeselectColors()
        {
            ColorPreset.SetColor(null);
            if (ColorPresetSpawner.GetSelected() == null) return;
            foreach (var t in ColorPresetSpawner.ColorPresets)
            {
                t.Deselect();
            }
        }

        public static void SelectTool(Tools tool)
        {
            switch (tool)
            {
                case Tools.Pencil:
                    Pencil.SelectWithoutAnimation();
                    break;
            }
            CurrentTool = tool;
        }

        public static SelectableTool GetTool(Tools tool)
        {
            switch (tool)
            {
                case Tools.Pencil:
                    return Pencil;
                case Tools.Eraser:
                    return Eraser;
            }
            return null;
        }
    }
}
