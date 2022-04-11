using System;
using System.Collections.Generic;
using _Scripts.SharedOverall;
using UnityEngine;

namespace _Scripts.Gameplay.Playing.UI
{
    public class TextHintInvoker : MonoBehaviour
    {
        public enum HintType           
        {                              
            Watch,                     
            Learn,                     
            Do,
            Mistake,
                               
            BaseStates,                
            PickColor,                 
            PickTool,              
                               
            Snapshot,                  
            Drawing       
        }
        
        private static readonly Dictionary<HintType, string> RuDictionary = new()
        {
            //TextHint
            
            { HintType.Watch, "Отличный выбор" },
            { HintType.Learn, "Смотри и учись" },
            { HintType.Do, "Теперь ты" },
            { HintType.Mistake, "Ошибаться это нормально" },
            
            { HintType.BaseStates, "Сначала досмотри" },
            { HintType.PickColor, "Ты забыл выбрать цвет" },
            { HintType.PickTool, "Сначала выбери один из инструментов" },
            
            { HintType.Snapshot, "Чтобы изменить цвет, нажми на него два раза подряд" },
            
            { HintType.Drawing, "Твой рисунок сохранен в галерею" },
        };
        
        private static readonly Dictionary<HintType, string> EngDictionary = new()
        {
            //TextHint
            
            { HintType.Watch, "Excellent choice" },
            { HintType.Learn, "Watch and learn" },
            { HintType.Do, "Now it's your turn" },
            { HintType.Mistake, "Oh, it's okay" },
            
            
            { HintType.BaseStates, "Check it out first" },
            { HintType.PickColor, "You forgot to choose a color" },
            { HintType.PickTool, "First choose one of the tools" },
            
            { HintType.Snapshot, "To change the color, click on it twice in a row" },
            
            { HintType.Drawing, "Your drawing is saved to the gallery" },
        };


        public static event Action<string, float> ShowHint;
        public static void Invoke(HintType type, float time)
        {
            var localHint = GetLocalizedHint(type);
            ShowHint?.Invoke(localHint, time);
        }
        
        private static string GetLocalizedHint(HintType index)
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