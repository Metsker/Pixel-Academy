using _Scripts.Gameplay.Release.Playing.Creating;
using _Scripts.Gameplay.Release.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Release.Playing.UI
{
    public class NextLevelButton : MonoBehaviour
    {
        private Button _button;
        private int _index;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (LevelCreator.groupScriptableObject == null)
            {
                _button.interactable = false;
                return;
            }
            _index = LevelCreator.groupScriptableObject.levels.FindIndex(l => l == LevelCreator.scriptableObject);
            if (_index + 1 != LevelCreator.groupScriptableObject.levels.Count && !LevelCreator.groupScriptableObject.levels[_index + 1].isLocked) return;
            _button.interactable = false;
        }

        public void NextLevel()
        {
            if (LevelCreator.groupScriptableObject == null) return;
            LevelCreator.scriptableObject = LevelCreator.groupScriptableObject.levels[_index+1];
            SceneTransitionManager.OpenScene(1);
        }
    }
}