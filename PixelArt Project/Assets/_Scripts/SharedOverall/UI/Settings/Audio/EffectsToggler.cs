using UnityEngine;

namespace _Scripts.SharedOverall.Settings
{
    public class EffectsToggler : AudioSetting
    {
        public override void ToggleClick()
        {
            Toggle("Effects");
        }
    }
}