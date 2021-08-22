using System;
using GameplayMod.Creating;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.ColorPresets
{
    public class ColorPresetSpawner : MonoBehaviour, ICreator
    {
        [SerializeField] private GameObject preset;
        private int _i;

        private void Start()
        {
            if (GameModManager.CurrentGameMod == GameModManager.GameMod.Play) return;
            for (var j = 0; j < 4; j++)
            {
                Create();
            }
        }

        public void Create()
        {
            switch (GameModManager.CurrentGameMod)
            {
                case GameModManager.GameMod.Play:
                {
                    foreach (var p in FindObjectsOfType<ColorPreset>())
                    {
                        Destroy(p.transform.parent.gameObject);
                    }
                    var count = LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].colorPresetStruct.Count;
                    for (var j = 0; j < count; j++)
                    {
                        var g = Instantiate(preset, transform);
                        g.name = LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].colorPresetStruct[j].name;
                        g.gameObject.transform.GetChild(0).GetComponent<Image>().color =
                            LevelCreator.scriptableObject.stageScriptableObjects[LevelCreator.Stage].colorPresetStruct[j].color;
                    }
                    break;
                }
                default:
                {
                    var g = Instantiate(preset, transform);
                    g.name = "Preset Color " + "(" + _i + ")";
                    _i++;
                    break;
                }
            }
        }
        
        public void Delete()
        {
            if (_i <= 0) return;
            _i--;
            Destroy(GameObject.Find("Preset Color " + "(" + _i + ")"));
        }
    }
}
