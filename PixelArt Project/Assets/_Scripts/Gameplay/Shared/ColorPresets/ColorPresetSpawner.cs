using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.ColorPresets;
using _Scripts.SharedOverall.Utility;
using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts.Gameplay.Shared.ColorPresets
{
    public class ColorPresetSpawner : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject preset;
        private GameObject _creatorInstance;
        private RectTransform _rectTransform;
        public static List<ColorPreset> ColorPresets { get; private set; }
        private ObjectPool<GameObject> _pool;

        private const int DrawingPresetsCount = 2;
        private int _i;

        private readonly Vector2 _vMin = new (0, 0.5f);
        private readonly Vector2 _vMax = new (1, 0.5f);
        

        private void Awake()
        {
            ColorPresets = new List<ColorPreset>();
            _rectTransform = GetComponent<RectTransform>();
            _pool = new ObjectPool<GameObject>(() => 
                    Instantiate(preset, transform),
                pt =>
                {
                    pt.SetActive(true);
                    _rectTransform.pivot = _pool.CountActive > 3 ? _vMin : _vMax;
                }, pt =>
                {
                    pt.name = "Released";
                    pt.SetActive(false);
                    _rectTransform.pivot = _pool.CountActive > 3 ? _vMin : _vMax;
                }, 
                Destroy,false,3,10);
        }

        private void Start()
        {
            if (GameModeManager.CurrentGameMode == GameModeManager.GameMode.Play) return;
            for (var i = 0; i < DrawingPresetsCount; i++)
            {
                Create();
            }
        }

        public void Create()
        {
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Play:
                {
                    var count = LevelCreator.GetCurrentStageScOb().colorPresetStruct.Count - _pool.CountActive;
                    switch (count)
                    {
                        case > 0:
                        {
                            for (var i = 0; i < count; i++)
                            {
                                var g = _pool.Get();
                                var cp = g.GetComponentInChildren<ColorPreset>();
                                ColorPresets.Add(cp);
                            }
                            break;
                        }
                        case < 0:
                        {
                            for (var i = 0; i < Mathf.Abs(count); i++)
                            {
                                var c = ColorPresets[i];
                                ColorPresets.Remove(c);
                                _pool.Release(c.transform.parent.gameObject);
                            }
                            break;
                        }
                    }
                    for (var i = 0; i < ColorPresets.Count; i++)
                    {
                        var c = ColorPresets[i];
                        var g = c.transform.parent.gameObject;
                        g.name = LevelCreator.GetCurrentStageScOb().colorPresetStruct[i].name;
                        c.SetImageColor(LevelCreator.GetCurrentStageScOb().colorPresetStruct[i].color);
                    }
                    break;
                }
                default:
                {
                    CreateInstance();
                    break;
                }
            }
        }
        public static ColorPreset GetSelected()
        {
            return ColorPresets.Find(cp => cp.IsSelected());
        }
        public static ColorPreset GetByColor(Color c)
        {
            return ColorPresets.Find(cp => cp.GetImageColor() == c);
        }
        
        private ColorPreset CreateInstance()
        {
            var g = _pool.Get();
            var cp = g.GetComponentInChildren<ColorPreset>();

            cp.SetImageColor(ColorRandomizer.GetRandomColor());

            g.transform.SetSiblingIndex(transform.childCount - (_creatorInstance ? 2 : 1));
            
            ColorPresets.Add(cp);
            g.name = $"Preset Color ({_i})";
            _i++;
            return cp;
        }
        
        public void CreatePreset()
        {
            var cp = CreateInstance();
            var t = transform;
            var position = t.position;
            position = new Vector3(((RectTransform)t).sizeDelta.x,position.y,position.z);
            t.position = position;
            PickerHandler.EnablePicker(cp.PickerAction, cp.colorImage);
            cp.SelectWithoutAnimation();
            ColorPreset.SetColor(cp.GetImageColor());
        }
        
        public void Delete()
        {
            var c = GetSelected();
            ColorPresets.Remove(c);
            _pool.Release(c.transform.parent.gameObject);
            ResetNames();
            PickerHandler.DisablePicker();
            ToolsManager.DeselectColors();
        }

        private void ResetNames()
        {
            _i = 0;
            foreach (var colorPreset in ColorPresets)
            {
                colorPreset.transform.parent.name = $"Preset Color ({_i})";
                _i++;
            }
        }
    }
}
