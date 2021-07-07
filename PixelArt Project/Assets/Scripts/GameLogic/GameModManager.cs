using UnityEngine;

namespace GameLogic
{
    public class GameModManager : MonoBehaviour
    {
        [SerializeField] private GameMod gameModSetter;
        public static GameMod CurrentGameMod { get; private set; }

        private void Awake()
        {
            CurrentGameMod = gameModSetter;
        }
        public enum GameMod
        {
            Gameplay,
            Editor
        }
    }
}