using System;
using _Scripts.Gameplay.Playing.Animating;
using _Scripts.Gameplay.Playing.Hints;
using _Scripts.Gameplay.Playing.UI;
using _Scripts.Gameplay.Release.Drawing;
using _Scripts.Gameplay.Shared.Tools.Instruments;
using _Scripts.Gameplay.Shared.UI;
using _Scripts.Menu.UI;
using _Scripts.SharedOverall.Ads;
using _Scripts.SharedOverall.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static _Scripts.SharedOverall.Dictionaries;

namespace _Scripts.SharedOverall.UI
{
    public class WarningUI : UIPanel
    {
        [SerializeField] private GameObject warningUI;
        [SerializeField] private Toggle showAgain;
        [SerializeField] private Image picture;

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
        
        private OpaqueHint _opaqueHint;
        private ClipHint _clipHint;
        private ClipManager _clipManager;
        private Vector2 _pictureStartSize;
        public static event Action<bool> SwitchBlur;

        private WarningType _warningType;
        
        public enum WarningType
        {
            Clear,
            Skip,
            OpaqueHint,
            ClipHint,
            Exit,
            Restart,
            UnlockLevel
        }

        private void Awake()
        {
            _pictureStartSize = ((RectTransform)picture.transform).sizeDelta;
        }

        private void Start()
        {
            if(GameModeManager.CurrentGameMode != GameModeManager.GameMode.Play) return;
            _opaqueHint = FindObjectOfType<OpaqueHint>();
            _clipHint = FindObjectOfType<ClipHint>();
            _clipManager = FindObjectOfType<ClipManager>();
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            ClearTool.ShowWarning += ShowWarning;
            ClipManager.ShowWarning += ShowWarning;
            BaseHint.ShowWarning += ShowWarning;
            MenuAwaiter.ShowWarning += ShowWarning;
            RestartAwaiter.ShowWarning += ShowWarning;
            StartLevelButton.ShowWarning += ShowWarning;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            ClearTool.ShowWarning -= ShowWarning;
            ClipManager.ShowWarning -= ShowWarning;
            BaseHint.ShowWarning -= ShowWarning;
            MenuAwaiter.ShowWarning -= ShowWarning;
            RestartAwaiter.ShowWarning -= ShowWarning;
            StartLevelButton.ShowWarning -= ShowWarning;
        }

        private void ShowWarning(WarningType type)
        {   
            SwitchBlur?.Invoke(true);
            _warningType = type;

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
                    CheckAdLoaded(this, EventArgs.Empty);
                    break;
                case WarningType.ClipHint:
                    CheckAdLoaded(this, EventArgs.Empty);
                    break;
                case WarningType.Exit:
                    labelText.SetText(GetLocalizedString("ExitLabel"));
                    purposeText.SetText(GetLocalizedString("ClearPurpose"));
                    buttonText.SetText(GetLocalizedString("ExitButton"));
                    picture.sprite = seallo;
                    break; 
                case WarningType.Restart:
                    labelText.SetText(GetLocalizedString("RestartLabel"));
                    purposeText.SetText(GetLocalizedString("ClearPurpose"));
                    buttonText.SetText(GetLocalizedString("RestartButton"));
                    picture.sprite = dinno;
                    break;
                case WarningType.UnlockLevel:
                    switch (StartLevelButton.CheckCost())
                    {
                        case true:
                            labelText.SetText(GetLocalizedString("UnlockLevelLabel"));
                            purposeText.SetText(GetLocalizedString("UnlockLevelPurpose") + StartLevelButton.LevelDataToUnlock.ScriptableObject.GetCost());
                            buttonText.SetText(GetLocalizedString("UnlockLevelButton"));
                            picture.sprite = seallo;
                            showAgain.interactable = false;
                            break;
                        case false:
                            labelText.SetText(GetLocalizedString("NoCoinsLabel"));
                            purposeText.SetText(GetLocalizedString("NoCoinsPurpose"));
                            buttonText.SetText(GetLocalizedString("NoCoinsButton"));
                            picture.sprite = dinno;
                            showAgain.interactable = false;
                            break;
                    }
                    break;
            }
            ImageAdjuster.Adjust((RectTransform)picture.transform, picture.sprite, _pictureStartSize);
        }

        public override void CloseUI()
        {
            if(!warningUI.activeSelf) return;
            SwitchBlur?.Invoke(false);   
            warningUI.gameObject.SetActive(false);
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
                    _clipManager.SkipClip();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("ClipWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.OpaqueHint:
                    switch (baseAdVideo.IsAdLoaded())
                    {
                        case true:
                            _opaqueHint.GetHintByAd();
                            break;
                        case false:
                            baseAdVideo.RequestRewarded();
                            return;
                    }
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("HintWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.ClipHint:
                    switch (baseAdVideo.IsAdLoaded())
                    {
                        case true:
                            _clipHint.GetHintByAd();
                            break;
                        case false:
                            baseAdVideo.RequestRewarded();
                            return;
                    }
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("HintWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.Exit:
                    MenuAwaiter.GotoMenu();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("ExitWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.Restart:
                    RestartAwaiter.Restart();
                    if (!showAgain.isOn) break;
                    PlayerPrefs.SetInt("RestartWarning", 1);
                    PlayerPrefs.Save();
                    break;
                case WarningType.UnlockLevel:
                    if (StartLevelButton.CheckCost())
                    {
                        StartLevelButton.LevelDataToUnlock.Unlock();
                        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) - StartLevelButton.LevelDataToUnlock.ScriptableObject.GetCost());
                        CoinsUI.UpdateCoinsUI();
                    }
                    break;
            }
            CloseUI();
        }

        public void CheckAdLoaded(object sender, EventArgs eventArgs)
        {
            if(!warningUI.activeSelf) return;
            switch (baseAdVideo.IsAdLoaded())
            {
                case true:
                    labelText.SetText(GetLocalizedString("HintLabel"));
                    buttonText.SetText(GetLocalizedString("HintButton"));
                    switch (_warningType)
                    {
                        case WarningType.OpaqueHint:
                            purposeText.SetText(GetLocalizedString("HintOpaquePurpose"));
                            picture.sprite = seallo;
                            break;
                        case WarningType.ClipHint:
                            purposeText.SetText(GetLocalizedString("HintClipPurpose"));
                            picture.sprite = chickko;
                            break;
                    }
                    break;
                case false:
                    labelText.SetText(GetLocalizedString("AdLoadingErrorLabel"));
                    purposeText.SetText(GetLocalizedString("AdLoadingErrorPurpose"));
                    buttonText.SetText(GetLocalizedString("AdLoadingErrorButton"));
                    picture.sprite = duckko;
                    baseAdVideo.RequestRewarded();
                    break;
            }
            ImageAdjuster.Adjust((RectTransform)picture.transform, picture.sprite, _pictureStartSize);
        }
    }
}