using GameplayMod.Creating;
using GameplayMod.Data;
using GameplayMod.Resulting;
using GeneralLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayMod.UI
{
    public class LevelStartButton : MonoBehaviour
    {
        private LevelData _levelData;

        private void Awake()
        {
            _levelData = GetComponentInParent<LevelData>();
        }

        public void Select()
        {
            GameModManager.isDebug = false;
            GameModManager.LevelGameMod = GameModManager.GameMod.Play;
            LevelCreator.scriptableObject = _levelData.scriptableObject;
            SceneManager.LoadScene(1);
        }
    }
}
