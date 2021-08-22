using DG.Tweening;
using GameplayMod.Creating;
using GeneralLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayMod.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        public void Select()
        {
            GameModManager.LevelGameMod = GameModManager.GameMod.Play;
            LevelCreator.scriptableObject = levelData.scriptableObject;
            SceneManager.LoadScene(1);
        }
    }
}
