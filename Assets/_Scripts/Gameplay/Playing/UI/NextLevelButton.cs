using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.UI
{
    public class NextLevelButton : MonoBehaviour
    {
        private Button _button;
        private int _index;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private LevelGroupScriptableObject GetGroup()
        {
            return LevelGroupsLoader.LevelGroupsLoaderSingleton.levelGroups[(int)LevelCreator.ScriptableObject.groupType];
        }
        
        private void Start()
        {
            if (GetGroup() == null)
            {
                _button.interactable = false;
                return;
            }
            _index = GetGroup().levels.FindIndex(l => l == LevelCreator.ScriptableObject);
            if (_index + 1 != GetGroup().levels.Count && !GetGroup().levels[_index + 1].isLocked) return;
            _button.interactable = false;
        }

        public void NextLevel()
        {
            if (GetGroup() == null) return;
            LevelCreator.ScriptableObject = GetGroup().levels[_index+1];
            SceneTransitionManager.OpenScene(SceneTransitionManager.Scenes.Play);
        }
    }
}