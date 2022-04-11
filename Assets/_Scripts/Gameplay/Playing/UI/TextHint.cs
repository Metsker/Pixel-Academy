using System;
using System.Collections;
using System.Text;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Creating;
using _Scripts.Gameplay.Playing.Hints;
using _Scripts.Gameplay.Recording.ScriptableObjectLogic;
using _Scripts.Gameplay.Release.Drawing;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Gameplay.Shared.Tools.Logic;
using _Scripts.SharedOverall;
using _Scripts.SharedOverall.Animating;
using _Scripts.SharedOverall.Audio;
using _Scripts.SharedOverall.Tools.Logic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.Playing.UI
{
    public class TextHint : MonoBehaviour
    {
        [SerializeField] private Image mascot;
        [SerializeField] private CanvasGroup canvasGroup;
        private TextMeshProUGUI _txt;
        private ToolAnimation _toolAnimation;
        private IEnumerator _hEnumerator;
        private static float _charDelay;
        private const float CharRef = 0.09f;
        private const float CharSkip = 0.01f;
        private const float FadeDuration = 0.3f;
        private static float _currentFadeTime; 
        public static bool IsAnimating { get; private set; }
        private Sequence _sequence;
    
        public static event Action<AudioManager.AudioClickType> PlaySound;

        private void Awake()
        {
            _txt = GetComponent<TextMeshProUGUI>();
            _toolAnimation = mascot.GetComponent<ToolAnimation>();
            _charDelay = CharRef;
            IsAnimating = false;
        }
        private void OnEnable()
        {
            TextHintInvoker.ShowHint += SetUpHint;
            ClipManager.SkipHint += SkipHint;
        }
        private void OnDisable()
        {
            TextHintInvoker.ShowHint -= SetUpHint;
            ClipManager.SkipHint -= SkipHint;
        }

        private void SetUpHint(string hint, float fadeTime)
        {
            if (_hEnumerator != null) 
            {
                _sequence.Kill();
                StopCoroutine(_hEnumerator);
            }
            _hEnumerator = ShowHint(hint,fadeTime);
            StartCoroutine(_hEnumerator);
        }

        private void SkipHint()
        {
            if (_hEnumerator != null)
            {
                _sequence.Kill();
                StopCoroutine(_hEnumerator);
            }
            canvasGroup.DOFade(0, FadeDuration);
            IsAnimating = false;
        }
        private IEnumerator ShowHint(string hint, float fadeTime)
        {
            IsAnimating = true;
            _charDelay = CharRef;
            _currentFadeTime = fadeTime;
        
            _txt.SetText("...");
        
            PlaySound?.Invoke(AudioManager.AudioClickType.Tool);
            _toolAnimation.PlayAnimation();
            
            yield return canvasGroup.DOFade(1, FadeDuration).WaitForCompletion();
            _txt.SetText("");
            var builder = new StringBuilder();
            foreach (var c in hint)
            {
                if (BlurManager.IsBlured())
                {
                    yield return new WaitUntil(() => !BlurManager.IsBlured());
                }
                builder.Append(c);
                _txt.SetText(builder.ToString());
                if (_charDelay>0)
                {
                    PlaySound?.Invoke(AudioManager.AudioClickType.Click);
                }
                yield return new WaitForSeconds(_charDelay);
            }
            
            if (fadeTime == 0)
            {
                IsAnimating = false;
                yield break;
            }
            for (float i = 0; i < _currentFadeTime; i+=Time.deltaTime)
            {
                yield return null;
            }
            IsAnimating = false;
            _sequence = DOTween.Sequence().Append(canvasGroup.DOFade(0, FadeDuration));
            yield return _sequence.WaitForCompletion();
        }

        public static void SkipTextAnim()
        {
            if (_charDelay == CharSkip)
            {
                _currentFadeTime = CharSkip;
                return;
            }
            _charDelay = CharSkip;
        }
    }
}