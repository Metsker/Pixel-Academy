using System.Collections.Generic;
using JetBrains.Annotations;

namespace _Scripts.SharedOverall
{
    public static class Dictionaries
    {
        private static readonly Dictionary<string, string> RuDictionary = new()
        {
            //Warning UI
            
            { "ClearLabel", "Очистить лист?" }, 
            { "ClearPurpose", "Весь прогресс рисования будет утерян. Придется начать заново." },
            { "ClearButton", "Стереть!" },
            
            { "SkipLabel", "Пропустить клип?" },
            { "SkipPurpose", "Если играете первый раз, то советуем не пропускать вспомогательный клип." },
            { "SkipButton", "Пропустить!" },

            { "HintLabel", "Получить подсказку?" },
            { "HintOpaquePurpose", "Откроет картинку за короткую рекламу." },
            { "HintClipPurpose", "Повторит клип с начала за короткую рекламу." },
            { "HintButton", "Да!" },
            
            { "ExitLabel", "Выйти в меню?" },
            { "ExitButton", "Выйти!" },
            
            { "RestartLabel", "Начать сначала?" },
            { "RestartButton", "Рестарт!" },
            
            { "UnlockLevelLabel", "Купить уровень?" },
            { "UnlockLevelPurpose", "Монет потребуется: " },
            { "UnlockLevelButton", "Купить!" },
            
            { "NoCoinsLabel", "Не хватает монет." },
            { "NoCoinsPurpose", "Зарабатывайте их, проходя уровни,\n или покупая в магазине." },
            { "NoCoinsButton", "Понятно!" },
            
            { "AdLoadingErrorLabel", "Реклама не загрузилась :(" },
            { "AdLoadingErrorPurpose", "Получить награду не получится. Проверьте подключение к интернету." },
            { "AdLoadingErrorButton", "Eщё попытка!" },
            
            //Reward Calculator
            
            { "Perfect", "Идеально!!!" },
            { "Great", "Отлично!" },
            { "Good", "Хорошо." },
            { "OK", "Не плохо." },
            { "Bad", "Плохо." },
            { "NotPassed", "Слишком много ошибок. :(" },
            
            { "CoinsEarned", "\nМонет заработано:" },
            
            //LevelCreator
            
            { "Stage", "Стадия\n" },

            //Shop
            
            { "LockedLabel", "Уровни" },
            
            //Notifications
            
            { "Title", "Время порисовать!" },
            { "SubtitleIOS", "Что-то особенное..." },
            { "Body", "Рисуй понемногу каждый день и ты быстро станешь профи" },
        };
        
        private static readonly Dictionary<string, string> EngDictionary = new()
        {
            //Warning UI
            
            { "ClearLabel", "Clear the canvas?" }, 
            { "ClearPurpose", "All drawing progress will be lost. You'll have to start over." },
            { "ClearButton", "Clear" },
            
            { "SkipLabel", "Skip the clip?" },
            { "SkipPurpose", "If you are playing for the first time, we advise you not to skip the supporting clip." },
            { "SkipButton", "Skip!" },
            
            { "HintLabel", "Get a hint?" },
            { "HintOpaquePurpose", "This will open the picture for a short ad." },
            { "HintClipPurpose", "This will repeat the clip from the beginning for a short ad." },
            { "HintButton", "Yes!" },

            { "ExitLabel", "Go to main menu?" },
            { "ExitButton", "Go!" },
            
            { "RestartLabel", "Start again?" },
            { "RestartButton", "Restart!" },
            
            { "UnlockLevelLabel", "Unlock level?" },
            { "UnlockLevelPurpose", "Coins will be required: " },
            { "UnlockLevelButton", "Unlock!" },
            
            { "NoCoinsLabel", "Not enough coins." },
            { "NoCoinsPurpose", "Earn them by completing levels,\n or by buying in the shop." },
            { "NoCoinsButton", "Got it!" },
            
            { "AdLoadingErrorLabel", "Ad loading fail :(" },
            { "AdLoadingErrorPurpose", "You will not be able to get a reward. Check your Internet connection." },
            { "AdLoadingErrorButton", "Try again!" },
            
            //Reward Calculator
            
            { "Perfect", "Perfect!!!" },
            { "Great", "Great!" },
            { "Good", "Good." },
            { "OK", "OK." },
            { "Bad", "Bad." },
            { "NotPassed", "Too many mistakes. :(" },
            
            { "CoinsEarned", "\nCoins earned:" },
            
            //LevelCreator
            
            { "Stage", "Stage\n" },

            //Shop
            
            { "LockedLabel", "Levels" },
            
            //Notifications
            
            { "Title", "It's time to draw!" },
            { "SubtitleIOS", "Something special..." },
            { "Body", "Draw a little every day and you will quickly become a pro" },
        };
        
        [CanBeNull]
        public static string GetLocalizedString(string index)
        {
            return LanguageManager.CurrentLanguage switch
            {
                LanguageManager.LocaleLanguages.English => EngDictionary[index],
                LanguageManager.LocaleLanguages.Russian => RuDictionary[index],
                _ => null
            };
        }
    }
}