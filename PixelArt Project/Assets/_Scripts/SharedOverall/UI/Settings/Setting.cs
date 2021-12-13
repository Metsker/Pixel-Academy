using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Settings
{   
    [RequireComponent(typeof(Image))]
    public abstract class Setting : MonoBehaviour
    {
        [SerializeField] protected List<Sprite> sprites; //0 - off; 1 - on
        protected Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public abstract void ToggleClick();
    }
}