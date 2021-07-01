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

        private void Awake()
        {
            AnimCount = 0;
            _pencil = FindObjectOfType<PencilTool>();
            _picker = FindObjectOfType<ColorPresetCreateButton>().picker;
            _image = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    StartCoroutine(ChangeColorAnim());
                    _pencil.SetColor(_image.color);
                    ToolMod.CurrentTool = ToolMod.Tools.Pencil;
                    
                    break;
                case PointerEventData.InputButton.Right:
                    _picker.onValueChanged.RemoveAllListeners();
                    _picker.gameObject.SetActive(true);
                    _picker.CurrentColor = _image.color;
                    _picker.onValueChanged.AddListener(color =>
                    {
                        _image.color = color;
                        _pencil.SetColor(_image.color);
                    });
                    ToolMod.CurrentTool = ToolMod.Tools.Pencil;
                    break;
            }
            if (eventData.clickCount == 2)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator ChangeColorAnim()
        {
            transform.DOPunchScale(Vector3.one/3, AnimDuration,1);
            if (GameState.CurrentState == GameState.State.Recording)
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
