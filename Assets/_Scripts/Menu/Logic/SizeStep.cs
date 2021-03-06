using System;
using _Scripts.Menu.Transition;
using _Scripts.SharedOverall.Audio;
using Assets._Scripts.Menu.Transition;
using TMPro;
using UnityEngine;

namespace _Scripts.Menu.Logic
{
    public class SizeStep : MonoBehaviour
    {
        public SizeStepSpawner.Side side { get; set; }
        public TextMeshProUGUI text { get; private set; }
        public static int XSide { get; set; }
        public static int YSide { get; set; }
        private const int Step = 50;
        private readonly Color _activeColor = Color.black;
        private readonly Color _inactiveColor = Color.gray;
        public static event Action<AudioManager.AudioClickType> PlaySound;
        
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CalculateStep(Step, _activeColor,int.Parse(text.text));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            CalculateStep(-Step, _inactiveColor,0);
            if (PageManager.CurrentPage == PageManager.Pages.Editor)
            {
                PlaySound?.Invoke(AudioManager.AudioClickType.UI);
            }
        }

        private void CalculateStep(int step, Color color, int value)
        {
            switch (side)
            {
                case SizeStepSpawner.Side.X:
                    XSide = value;
                    break;
                case SizeStepSpawner.Side.Y:
                    YSide = value;
                    break;
            }
            text.color = color;
            text.fontSize += step;
        }
    }
}
