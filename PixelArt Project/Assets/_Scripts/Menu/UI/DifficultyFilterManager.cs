using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.UI
{
    public class DifficultyFilterManager: MonoBehaviour
    {
        public static Difficulties currentDifficulty { get; set; } = Difficulties.None;
        public DifficultyFilterButton[] filters;
        
        public enum Difficulties
        {
            Easy,
            Medium,
            Hard,
            None
        }
        
        private void Start()
        {
            switch (currentDifficulty)
            {
                case Difficulties.None:
                    break;
                default:
                    filters[(int)currentDifficulty].Switch(currentDifficulty);
                    break;
            }
        }
    }
}