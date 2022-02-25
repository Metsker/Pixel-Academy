using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Recording.UI
{
    public class LevelGroupsManager : MonoBehaviour
    {
        private static TMP_Dropdown _dropdown;
        public static GroupType SelectedGroupType = GroupType.Animals;
        
        [Serializable]
        public enum GroupType
        {
            Animals,
            Crops,
            Food,
            Decor,
            Minecraft,
            Pokemons,
            Various
        }

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            PopulateDropDownWithEnum(_dropdown, SelectedGroupType);
            _dropdown.onValueChanged.AddListener(arg0 => DropdownValueChanged(_dropdown));
        }

        private void DropdownValueChanged(TMP_Dropdown change)
        {
            SelectedGroupType = (GroupType)change.value;
        }

        private static void PopulateDropDownWithEnum(TMP_Dropdown dropdown, Enum targetEnum)
        {
            var enumType = targetEnum.GetType();
            var newOptions = new List<TMP_Dropdown.OptionData>();
 
            for(var i = 0; i < Enum.GetNames(enumType).Length; i++)
            {
                newOptions.Add(new TMP_Dropdown.OptionData(Enum.GetName(enumType, i)));
            }
            
            dropdown.ClearOptions();
            dropdown.AddOptions(newOptions);
        }

        public static void SetGroupType(GroupType newType)
        {
            SelectedGroupType = newType;
            _dropdown.SetValueWithoutNotify((int)newType);
        }
    }
}