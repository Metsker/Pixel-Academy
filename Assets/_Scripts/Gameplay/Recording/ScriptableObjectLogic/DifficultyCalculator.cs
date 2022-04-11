using static _Scripts.Menu.UI.DifficultyFilterManager;

namespace _Scripts.Gameplay.Recording.ScriptableObjectLogic
{
    public static class DifficultyCalculator
    {
        public static Difficulties Calculate(int size)
        {
            switch (size)
            {
                case var _ when size <= 8*8:
                    return Difficulties.Easy;
                case var _ when size > 8*8 && size < 12*12:
                    return Difficulties.Medium;
                case var _ when size >= 12*12:
                    return Difficulties.Hard;
            }
            return Difficulties.None;
        }
    }
}
