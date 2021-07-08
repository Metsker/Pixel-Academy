using System;
using System.Collections;
using DG.Tweening;
using GameLogic;
using HSVPicker;
using MapEditor.Recording;
using Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor.ColorPresets
{
    public class ColorPreset : MonoBehaviour, IPointerClickHandler
    {
        private PencilTool _pencil;
        private Image _image;

        private const float AnimDuration = 0.6f;
        public static int AnimCount;
        public bool changed { get; set; }
        private bool _animPlaying;
        public ColorPresetStruct PresetStruct { get; private set; }
        public static event Action<float> TakeSnapshot;
        public static event Action<UnityAction<Color>, Image> EnablePicker;
        public static event Action DisablePicker;
        
        public UnityAction<Color> pickerAction;


        private void Awake()
        {
            AnimCount = 0;
            _pencil = FindObjectOfType<PencilTool>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            if (GameModManager.CurrentGameMod != GameModManager.GameMod.Editor) return;
            pickerAction = color =>
            {
                _image.color = color;
                if (GameStateManager.CurrentGameState != GameStateManager.GameState.Recording)
                {
                    _pencil.SetColor(color);
                }
            };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (!_animPlaying) StartCoroutine(ChangeColorAnim());
                    _pencil.SetColor(_image.color);
                    ToolMod.CurrentTool = ToolMod.Tools.Pencil;
                    if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording) changed = true;
                    break;
                
                case PointerEventData.InputButton.Right:
                    if (GameModManager.CurrentGameMod != GameModManager.GameMod.Editor) break;
                    if (changed)
                    {
                        Debug.LogWarning("Незьзя поменять анимированный цвет");
                        break;
                    }
                    DisablePicker?.Invoke();
                    EnablePicker?.Invoke(pickerAction, _image);
                    break;
            }
        }

        private IEnumerator ChangeColorAnim()
        {
            _animPlaying = true;
            transform.DOPunchScale(Vector3.one/3, AnimDuration,1);
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
            {
                AnimCount++;
                float i = 0;
                while (i<AnimDuration)
                {
                    TakeSnapshot?.Invoke(Time.deltaTime);
                    i += Time.deltaTime;
                    yield return null;
                }
                AnimCount--;
                _animPlaying = false;
            }
            else
            {
                yield return new WaitForSeconds(AnimDuration);
                _animPlaying = false;
            }
        }
        
        public void AssignStruct()
        {
            PresetStruct = new ColorPresetStruct(_image.color, gameObject.name);
        }
    }
}
