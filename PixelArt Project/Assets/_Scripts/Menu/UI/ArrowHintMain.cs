
namespace _Scripts.Menu.UI
{
    public class ArrowHintMain : BaseArrowHint
    {
        private static bool _isShown;

        protected override bool IsShown()
        {
            return _isShown;
        }
        protected override void SetShown()
        {
            _isShown = true;
        }
    }
}