using System.Linq;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Tools.Instruments;
using UnityEngine;

namespace _Scripts.SharedOverall.Tools.Logic
{
    public class ToolsManager : MonoBehaviour
    {
        public static Tools CurrentTool { get; set; } = Tools.None;
        public static readonly Color ColorZero = new (0, 0, 0, 0);
        private static FillingTool _fillingTool;
        private static PipetteTool _pipetteTool;

        private void Awake()
        {
            _fillingTool = FindObjectOfType<FillingTool>();
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            _pipetteTool = FindObjectOfType<PipetteTool>();
        }

        public enum Tools
        {
            None,
            Pencil,
            Eraser,
            Filler,
            Pipette
        }

        public static void DeselectTools(ISelectable item)
        {
            foreach (var t in FindObjectsOfType<MonoBehaviour>().OfType<ISelectable>().Where(t => t.IsSelected() && t != item))
            {
                t.Deselect();
            }
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play || CurrentTool == Tools.Pencil) return;
            PickerHandler.DisablePicker();
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
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            DeselectPipette();
        }

        public static void DeselectPipette()
        {
            if (!_pipetteTool.IsSelected()) return;
            _pipetteTool.Deselect();

        }

        public static bool IsPipetteSelected()
        {
            return _pipetteTool.IsSelected();
        }
    }
}
