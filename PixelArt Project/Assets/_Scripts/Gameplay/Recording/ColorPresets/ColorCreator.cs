using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Tools.Logic;
using UnityEngine.EventSystems;

namespace _Scripts.Gameplay.Recording.ColorPresets
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
