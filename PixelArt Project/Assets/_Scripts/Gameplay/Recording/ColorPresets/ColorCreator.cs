using _Scripts.GeneralLogic.ColorPresets;
using _Scripts.GeneralLogic.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.EditorMod.ColorPresets
{
    public class ColorCreator : BaseTool, IPointerClickHandler
    {
        private ColorPresetSpawner _colorPresetSpawner;

        private new void Awake()
        {
            base.Awake();
            _colorPresetSpawner = transform.parent.parent.TryGetComponent<ColorPresetSpawner>
                (out var cps) ? cps : FindObjectOfType<ColorPresetSpawner>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(ToolsManager.Tools.Pencil);
            _colorPresetSpawner.CreatePreset();
        }
    }
}
