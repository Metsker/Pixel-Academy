using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Recording.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall
{
    public class LevelGroupsLoader : MonoBehaviour
    {
        public static LevelGroupsLoader levelGroupsLoader { get; private set; }
        public List<LevelGroupScriptableObject> levelGroups;
        public LevelGroupScriptableObject newsLevelGroup;

        private void Awake()
        {
            levelGroupsLoader = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            foreach (var g in levelGroups)
            {
                for (var i = g.levels.Count - 1; i > -1; i--)
                {
                    if (g.levels[i] != null) continue;
                    g.levels.RemoveAt(i);
                }
            }
            for (var i = newsLevelGroup.levels.Count - 1; i > -1; i--)
            {
                if (newsLevelGroup.levels[i] == null)
                    newsLevelGroup.levels.RemoveAt(i);
            }       
#endif
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)
                && GameModeManager.CurrentGameMode != GameModeManager.GameMode.Record)
            {
                SceneManager.LoadSceneAsync(1);
            }
        }
#if UNITY_EDITOR 
        public static bool TryFindLevelInCurrentGroup(string levelName, out LevelScriptableObject level)
        {
            level = LevelGroupScriptableObject.GetCurrentLevelGroupScrObj().levels
                .Find(n => n.name == levelName);
            return level != null;
        }
        public static bool TryFindLevelInOtherGroups(string levelName, out LevelScriptableObject level)
        {
            level = levelGroupsLoader.levelGroups.Where(g => g != LevelGroupScriptableObject.GetCurrentLevelGroupScrObj()).
                SelectMany(g => g.levels.Where
                (l => l.name == levelName)).FirstOrDefault();
            return level != null;
        }
#endif
    }
}