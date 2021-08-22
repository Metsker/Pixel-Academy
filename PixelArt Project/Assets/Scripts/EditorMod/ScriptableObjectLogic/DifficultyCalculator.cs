using UnityEngine;

namespace EditorMod.ScriptableObjectLogic
{
    public static class DifficultyCalculator
    {
        public enum Difficulties
        {
            Easy,
            Medium,
            Hard
        }
        public static Difficulties Calculate(int size)
        {
            switch (size)
            {
                case var _ when size < 8*8:
                    return Difficulties.Easy;
                case var _ when size >= 8*8 && size < 16*16:
                    return Difficulties.Medium;
                case var _ when size >= 16*16:
                    return Difficulties.Hard;
            }
            return Difficulties.Easy;
        }
    }
}
