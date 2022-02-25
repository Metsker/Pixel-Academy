using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.UI.Settings
{   
    [RequireComponent(typeof(Image))]
    public abstract class Setting : MonoBehaviour
    {
        [SerializeField] protected List<Sprite> sprites; //0 - off; 1 - on
        protected Image image;

        protected void Awake()
        {
            image = GetComponent<Image>();
        }
        public abstract void ToggleClick();
    }
}