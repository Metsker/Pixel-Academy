using System.Collections.Generic;
using _Scripts.Gameplay.Release.Playing.Creating;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _Scripts.SharedOverall.ColorPresets
{
    public class ColorPresetSpawner : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject preset;
        [SerializeField] private GameObject colorCreator;
        private GameObject _creatorInstance;
        public static List<ColorPreset> colorPresets { get; private set; }
        private ObjectPool<GameObject> _pool;

        private const int DrawingPresetsCount = 2;
        private int _i;

        private void Awake()
        {
            colorPresets = new List<ColorPreset>();
            _pool = new ObjectPool<GameObject>(() => 
                    Instantiate(preset, transform),
                pt =>
                {
                    pt.SetActive(true); 
                }, pt =>
                {
                    pt.name = "Released";
                    pt.SetActive(false);
                }, 
                Destroy,false,3,10);
        }

        private void Start()
        {
            switch (GameModeManager.CurrentGameMode)
            {
                case GameModeManager.GameMode.Play:
                    return;
                case GameModeManager.GameMode.Paint:
                    _creatorInstance = Instantiate(colorCreator, transform);
                    _creatorInstance.name = "Color Creator";
                    break;
            }
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
                                colorPresets.Add(cp);
                            }
                            break;
                        }
                        case < 0:
                        {
                            for (var i = 0; i < Mathf.Abs(count); i++)
                            {
                                var c = colorPresets[i];
                                colorPresets.Remove(c);
                                _pool.Release(c.transform.parent.gameObject);
                            }
                            break;
                        }
                    }
                    for (var i = 0; i < colorPresets.Count; i++)
                    {
                        var c = colorPresets[i];
                        var g = c.transform.parent.gameObject;
                        g.name = LevelCreator.GetCurrentStageScOb().colorPresetStruct[i].name;
                        c.image.color = LevelCreator.GetCurrentStageScOb().colorPresetStruct[i].color;
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
            return colorPresets.Find(cp => cp.IsSelected());
        }
        public static ColorPreset GetByColor(Color c)
        {
            return colorPresets.Find(cp => cp.image.color == c);
        }
        
        private ColorPreset CreateInstance()
        {
            var g = _pool.Get();
            var cp = g.GetComponentInChildren<ColorPreset>();
            
            cp.image.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f));

            g.transform.SetSiblingIndex(transform.childCount - (_creatorInstance ? 2 : 1));
            
            colorPresets.Add(cp);
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
            PickerHandler.EnablePicker(cp.pickerAction, cp.image);
            cp.Select();
        }
        
        public void Delete()
        {
            var c = GetSelected();
            colorPresets.Remove(c);
            _pool.Release(c.transform.parent.gameObject);
            ResetNames();
            PickerHandler.DisablePicker();
        }

        private void ResetNames()
        {
            _i = 0;
            foreach (var colorPreset in colorPresets)
            {
                colorPreset.transform.parent.name = $"Preset Color ({_i})";
                _i++;
            }
        }
    }
}
