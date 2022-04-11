using UnityEngine;

namespace _Scripts.Menu.UI
{
    public class DifficultyFilterManager: MonoBehaviour
    {
        public static Difficulties CurrentDifficulty { get; set; } = Difficulties.None;
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
            switch (CurrentDifficulty)
            {
                case Difficulties.None:
                    break;
                default:
                    filters[(int)CurrentDifficulty].Switch(CurrentDifficulty);
                    break;
            }
        }
    }
}