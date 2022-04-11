using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Recording.UI;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.SharedOverall
{
    public class LevelGroupsLoader : MonoBehaviour
    {
        public static LevelGroupsLoader LevelGroupsLoaderSingleton { get; private set; }
        public List<LevelGroupScriptableObject> levelGroups;

        private void Awake()
        {
            LevelGroupsLoaderSingleton = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
            
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            foreach (var g in levelGroups)
            {
                if (g.groupType == LevelGroupsManager.GroupType.News) continue;
                
                for (var i = g.levels.Count - 1; i > -1; i--)
                {
                    if (g.levels[i] != null) continue;
                    g.levels.RemoveAt(i);
                }
                g.levels = g.levels.OrderBy(l => l.difficulty).Distinct().ToList();
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
            level = LevelGroupsLoaderSingleton.levelGroups.Where(g => g != LevelGroupScriptableObject.GetCurrentLevelGroupScrObj()).
                SelectMany(g => g.levels.Where
                (l => l.name == levelName)).FirstOrDefault();
            return level != null;
        }
#endif
        public static void UpdateGroupOrderByStars(LevelGroupScriptableObject group)
        {
            var g = LevelGroupsLoaderSingleton.levelGroups.Find(j => j == group);
            
            g.levels = g.levels.OrderByDescending(l => l.stars).ToList();
        }
    }
}