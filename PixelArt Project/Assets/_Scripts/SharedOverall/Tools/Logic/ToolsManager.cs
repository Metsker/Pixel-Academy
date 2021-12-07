using System;
using System.Linq;
using _Scripts.GeneralLogic.ColorPresets;
using _Scripts.GeneralLogic.Tools.Instruments;
using UnityEngine;

namespace _Scripts.GeneralLogic.Tools.Logic
{
    public class ToolsManager : MonoBehaviour
    {
        public static Tools CurrentTool { get; set; } = Tools.None;
        public static readonly Color ColorZero = new (0, 0, 0, 0);
        private static FillingTool _fillingTool;

        private void Awake()
        {
            _fillingTool = FindObjectOfType<FillingTool>();
        }

        public enum Tools
        {
            None,
            Pencil,
            Eraser,
            Filler,
            Pipette
        }

        public static void DeselectTools()
        {
            foreach (var t in FindObjectsOfType<MonoBehaviour>().OfType<ISelectable>().Where(t => t.IsSelected()))
            {
                t.Deselect();
            }
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play || CurrentTool == Tools.Pencil) return;
            PickerHandler.DisablePicker();
        }

        public static void DeselectInstruments()
        {
            _fillingTool.Deselect();
        }
    }
}
