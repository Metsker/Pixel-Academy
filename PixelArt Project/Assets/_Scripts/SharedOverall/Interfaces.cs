using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall
{
    public interface ICreator
    {
        void Create();
    }
    public interface ISelectable
    {
        void Select();
        void Deselect();
        bool IsSelected();
        Color GetDeselectedColor();
        Image GetSelectImage();
    }
}