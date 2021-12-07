using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GameplayMod.Animating;
using _Scripts.GameplayMod.Drawing;
using _Scripts.GameplayMod.Hints;
using _Scripts.GeneralLogic;
using _Scripts.GeneralLogic.Ads;
using _Scripts.GeneralLogic.Menu.UI;
using _Scripts.GeneralLogic.Tools;
using _Scripts.GeneralLogic.Tools.Instruments;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GameplayMod.UI
{
    public class WarningUI : MonoBehaviour
    {
        [SerializeField] private GameObject warningUI;
        [SerializeField] private Toggle showAgain;
        [SerializeField] private Image picture;
        [SerializeField] private ClipManager clipManager;

        [SerializeField] private BaseAdVideo baseAdVideo;
        
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI purposeText;
        [SerializeField] private TextMeshProUGUI buttonText;
        
        [SerializeField] private Sprite doggo;
        [SerializeField] private Sprite froggo;
        [SerializeField] private Sprite duckko;
        [SerializeField] private Sprite chickko;
        [SerializeField] private Sprite dinno;
        [SerializeField] private Sprite seallo;

        private readonly Dictionary<string, string> _ruDictionary = new()
        {
            { "ClearLabel", "Очистить лист?" }, 
            { "ClearPurpose", "Весь прогресс рисования будет утерян. Придется начать заново." },
            { "ClearButton", "Стереть!" },
            
            { "SkipLabel", "Пропустить клип?" },
            { "SkipPurpose", "Если играете первый раз, то советуем не пропускать вспомогательный клип." },
            { "SkipButton", "Пропустить!" },
            
            { "HintLabel", "Посмотреть подсказку?" },
            { "HintAdLabel", "Не хватает токенов." },
            { "HintCost", "\n Стоимость:" },
            { "HintOpaquePurpose", "Откроет картинку." },
            { "HintClipPurpose", "Повторит клип с начала." },
            { "HintAdPurpose", "Зарабатывайте их, проходя уровни,\n или просто посмотрите рекламу." },
            { "HintButton", "Посмотреть!" },
            
            { "DrawingTipLabel", "Совет" },
            { "DrawingTipPurpose", "Чтобы изменить цвет, нажми на него два раза подряд." },
            { "DrawingTipButton", "Понятно!" },
            
            { "ExitLabel", "Выйти в меню?" },
            { "ExitButton", "Выйти!" },
            
            { "UnlockLevelLabel", "Разблокировать уровень?" },
            { "UnlockLevelPurpose", "Посмотрите рекламу чтобы открыть уровень навсегда." },
            { "UnlockLevelButton", "Посмотреть!" },
            
            { "UnlockLevelLabelError", "Реклама не загрузилась :(" },
            { "UnlockLevelPurposeError", "Получить награду не получится. Проверьте подключение к интернету." },
            { "UnlockLevelButtonError", "Eщё попытка!" }
        };
        
        private readonly Dictionary<string, string> _engDictionary = new()
        {
            { "ClearLabel", "Clear the canvas?" }, 
            { "ClearPurpose", "All drawing progress will be lost. You'll have to start over." },
            { "ClearButton", "Clear" },
            
            { "SkipLabel", "Skip the clip?" },
            { "SkipPurpose", "If you are playing for the first time, we advise you not to skip the supporting clip." },
            { "SkipButton", "Skip!" },
            
            { "HintLabel", "Watch a hint?" },
            { "HintAdLabel", "Not enough tokens :(" },
            { "HintCost", "\n Price:" },
            { "HintOpaquePurpose", "This will open the picture." },
            { "HintClipPurpose", "This will repeat the clip from the beginning." },
            { "HintAdPurpose", "Earn them by completing levels, or watch the ad to get a hint." },
            { "HintButton", "Watch!" },
            
            { "DrawingTipLabel", "Tip" },
            { "DrawingTipPurpose", "To change the color, click on it twice in a row." },
            { "DrawingTipButton", "Clear!" },
            
            { "ExitLabel", "Go to main menu?" },
            { "ExitButton", "Go!" },
            
            { "UnlockLevelLabel", "Unlock level?" },
            { "UnlockLevelPurpose", "Watch the ad to unlock the level forever." },
            { "UnlockLevelButton", "Watch!" },
            
            { "UnlockLevelLabelError", "Ad loading fail :(" },
            { "UnlockLevelPurposeError", "You will not be able to get a reward. Check your Internet connection." },
            { "UnlockLevelButtonError", "Try again!" }
        };
        
        private OpaqueHint _opaqueHint;
        private ClipHint _clipHint;
        private MenuButton _menuButton;
        private Vector2 _pictureStartSize;
        public static event Action SwitchBlur;

        private WarningType _warningType;
        public enum WarningType
        {
            Clear,
            Skip,
            OpaqueHint,
            ClipHint,
            DrawingTip1,
            Exit,
            UnlockLevel
        }

        private void Awake()
        {
            _pictureStartSize = ((RectTransform)picture.transform).sizeDelta;
        }

        private void Start()
        {
            _menuButton = FindObjectOfType<MenuButton>();
            if(GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) return;
            _opaqueHint = FindObjectOfType<OpaqueHint>();
            _clipHint = FindObjectOfType<ClipHint>();
        }

        private void OnEnable()
        {
            ClearTool.ShowWarning += ShowWarning;
            ClipManager.ShowWarning += ShowWarning;
            BaseHint.ShowWarning += ShowWarning;
            StartDrawing.ShowTip += ShowWarning;
            MenuButton.ShowWarning += ShowWarning;
            LevelStartButton.ShowWarning += ShowWarning;

            MenuButton.IsWarningActive += IsWarningActive;
            MenuButton.CloseWarning += CloseWarning;
        }

        private void OnDisable()
        {
            ClearTool.ShowWarning -= ShowWarning;
            ClipManager.ShowWarning -= ShowWarning;
            BaseHint.ShowWarning -= ShowWarning;
            StartDrawing.ShowTip -= ShowWarning;
            MenuButton.ShowWarning -= ShowWarning;
            LevelStartButton.ShowWarning -= ShowWarning;
            
            MenuButton.IsWarningActive -= IsWarningActive;
            MenuButton.CloseWarning -= CloseWarning;
        }

        private void ShowWarning(WarningType type)
        {   
            SwitchBlur?.Invoke();
            _warningType = type;
            
            if (clipManager != null && GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                clipManager.SwitchPause(false);
            }
            
            if (!showAgain.interactable)
            {
                showAgain.interactable = true;
            }
            
            warningUI.gameObject.SetActive(true);
            
            switch (_warningType)
            {
                case WarningType.Clear:
                    labelText.SetText(GetLocalizedString("ClearLabel"));
                    purposeText.SetText(GetLocalizedString("ClearPurpose"));
                    buttonText.SetText(GetLocalizedString("ClearButton"));
                    picture.sprite = doggo;
                    break;
                case WarningType.Skip:
                    labelText.SetText(GetLocalizedString("SkipLabel"));
                    purposeText.SetText(GetLocalizedString("SkipPurpose"));
                    buttonText.SetText(GetLocalizedString("SkipButton"));
                    picture.sprite = froggo;
                    break;
                case WarningType.OpaqueHint:
                    switch (_opaqueHint.HaveTokens())
                    {
                        case true:
                            labelText.SetText(GetLocalizedString("HintLabel"));
                            var costString = $"{GetLocalizedString("HintCost")} {_opaqueHint.GetCost()}";
                            purposeText.SetText(GetLocalizedString("HintOpaquePurpose") + costString);
                            buttonText.SetText(GetLocalizedString("HintButton"));
                            picture.sprite = chickko;
                            break;
                        case false:
                            showAgain.interactable = false;
                            CheckAdLoaded(this, EventArgs.Empty);
                            break;
                    }
                    break;
                case WarningType.ClipHint:
                    switch (_clipHint.HaveTokens())
                    {
                        case true:
                            labelText.SetText(GetLocalizedString("HintLabel"));
                            var costString = $"{GetLocalizedString("HintCost")} {_clipHint.GetCost()}";
                            purposeText.SetText(GetLocalizedString("HintClipPurpose") + costString);
                            buttonText.SetText(GetLocalizedString("HintButton"));
                            picture.sprite = chickko;
                            break;
                        case false:
                            showAgain.interactable = false;
                            CheckAdLoaded(this, EventArgs.Empty);
                            break;
                    }
                    break;
                case WarningType.DrawingTip1:
                    labelText.SetText(GetLocalizedString("DrawingTipLabel"));
                    purposeText.SetText(GetLocalizedString("DrawingTipPurpose"));
                    buttonText.SetText(GetLocalizedString("DrawingTipButton"));
                    picture.sprite = dinno;
                    break;
                case WarningType.Exit:
                    labelText.SetText(GetLocalizedString("ExitLabel"));
                    purposeText.SetText(GetLocalizedString("ClearPurpose"));
                    buttonText.SetText(GetLocalizedString("ExitButton"));
                    picture.sprite = seallo;
                    break;
                case WarningType.UnlockLevel:
                    showAgain.interactable = false;
                    CheckAdLoaded(this, EventArgs.Empty);
                    break;
            }
            ImageAdjuster.Adjust((RectTransform)picture.transform, picture.sprite, _pictureStartSize);
        }
        public void CloseWarning()
        {
            SwitchBlur?.Invoke();   
            warningUI.gameObject.SetActive(false);
            if (clipManager != null && GameStateManager.CurrentGameState == GameStateManager.GameState.Animating)
            {
                clipManager.SwitchPause(true);
            }
            if (showAgain.isOn == false) return;
            showAgain.isOn = false;
        }
        public void OnButtonClick()
        {
            switch (_warningType)
            {
                case WarningType.Clear:
                    ClearTool.Clear();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("ClearWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.Skip:
                    clipManager.SkipClip();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("ClipWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.OpaqueHint:
                    switch (_opaqueHint.HaveTokens())
                    {
                        case true:
                            _opaqueHint.BuyHint();
                            break;
                        case false when baseAdVideo.IsAdLoaded():
                            _opaqueHint.WatchAd();
                            break;
                        case false when !baseAdVideo.IsAdLoaded():
                            baseAdVideo.RequestRewarded();
                            return;
                    }
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("HintWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.ClipHint:
                    switch (_clipHint.HaveTokens())
                    {
                        case true:
                            _clipHint.BuyHint();
                            break;
                        case false when baseAdVideo.IsAdLoaded():
                            _clipHint.WatchAd();
                            break;
                        case false when !baseAdVideo.IsAdLoaded():
                            baseAdVideo.RequestRewarded();
                            return;
                    }
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("HintWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.DrawingTip1:
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("DrawingTipWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.Exit:
                    _menuButton.GotoMenu();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("ExitWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.UnlockLevel:
                    switch (baseAdVideo.IsAdLoaded())
                    {
                        case true:
                            baseAdVideo.ShowVideo();
                            break;
                        case false:
                            baseAdVideo.RequestRewarded();
                            return;
                    }
                    break;
            }
            CloseWarning();
        }

        public bool IsWarningActive()
        {
            return warningUI.activeSelf;
        }

        public void CheckAdLoaded(object sender, EventArgs eventArgs)
        {
            if(!warningUI.activeSelf) return;
            switch (baseAdVideo.IsAdLoaded())
            {
                case true:
                    switch (_warningType)
                    {
                        case WarningType.ClipHint:
                            labelText.SetText(GetLocalizedString("HintAdLabel"));
                            purposeText.SetText(GetLocalizedString("HintAdPurpose"));
                            buttonText.SetText(GetLocalizedString("HintButton"));
                            picture.sprite = dinno;
                            break;
                        case WarningType.OpaqueHint:
                            labelText.SetText(GetLocalizedString("HintAdLabel"));
                            purposeText.SetText(GetLocalizedString("HintAdPurpose"));
                            buttonText.SetText(GetLocalizedString("HintButton"));
                            picture.sprite = dinno;
                            break;
                        case WarningType.UnlockLevel:
                            labelText.SetText(GetLocalizedString("UnlockLevelLabel"));
                            purposeText.SetText(GetLocalizedString("UnlockLevelPurpose"));
                            buttonText.SetText(GetLocalizedString("UnlockLevelButton"));
                            picture.sprite = dinno;
                            break;
                    }
                    break;
                case false:
                    labelText.SetText(GetLocalizedString("UnlockLevelLabelError"));
                    purposeText.SetText(GetLocalizedString("UnlockLevelPurposeError"));
                    buttonText.SetText(GetLocalizedString("UnlockLevelButtonError"));
                    picture.sprite = duckko;
                    baseAdVideo.RequestRewarded();
                    break;
            }
            ImageAdjuster.Adjust((RectTransform)picture.transform, picture.sprite, _pictureStartSize);
        }

        private string GetLocalizedString(string index)
        {
            return Application.systemLanguage == SystemLanguage.Russian ? _ruDictionary[index] : _engDictionary[index];
        }
    }
}