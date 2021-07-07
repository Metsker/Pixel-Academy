using System;
using System.Collections;
using DG.Tweening;
using GameLogic;
using HSVPicker;
using MapEditor.Recording;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor.ColorPresets
{
    public class ColorPreset : MonoBehaviour, IPointerClickHandler
    {
        private ColorPicker _picker;
        private PencilTool _pencil;
        private Image _image;
        private const float AnimDuration = 0.6f;
        public static int AnimCount;
        public static event Action<float> TakeSnapshot;
        public ColorPresetStruct PresetStruct { get; private set; }
        private bool _changed;

        private void Awake()
        {
            AnimCount = 0;
            _pencil = FindObjectOfType<PencilTool>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Editor)
            {
                _picker = FindObjectOfType<ColorPresetCreateButton>().picker;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    StartCoroutine(ChangeColorAnim());
                    _pencil.SetColor(_image.color);
                    ToolMod.CurrentTool = ToolMod.Tools.Pencil;
                    if (GameStateManager.CurrentGameState == GameStateManager.GameState.Recording)
                    {
                        _changed = true; 
                    }
                    break;
                case PointerEventData.InputButton.Right:
                    if (_changed)
                    {
                        Debug.LogWarning("Цвет уже был использован");
                        break;
                    }
                    _picker.onValueChanged.RemoveAllListeners();
                    _picker.gameObject.SetActive(true);
                    _picker.CurrentColor = _image.color;
                    _picker.onValueChanged.AddListener(color =>
                    {
                        _image.color = color;
                    });
                    break;
            }
            if (eventData.clickCount == 2)
            {
                Destroy(gameObject);
            }
        }

        public void AssignStruct()
        {
            PresetStruct = new ColorPresetStruct(_image.color, gameObject.name);
        }
        
        private IEnumerator ChangeColorAnim()
        {
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
            }
        }
    }
}
