using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GeneralLogic.Tools.Logic
{
    public abstract class SelectableTool : BaseTool, ISelectable
    {
        [SerializeField] private Image selectImage;
        private readonly Color _selectedColor = Color.white;
        private readonly Color _deselectedColor = new Vector4(1,1,1,0);
        
        public void Select()
        {
            ToolsManager.DeselectTools();
            GetSelectImage().color = GetSelectedColor();
        }
        public void SelectWithAnimation()
        {
            toolAnimation.PlayAnimation();
            ToolsManager.DeselectTools();
            GetSelectImage().color = GetSelectedColor();
        }
        public void Deselect()
        {
            GetSelectImage().color = GetDeselectedColor();
        }
        public virtual bool IsSelected()
        {
            return GetSelectImage().color == GetSelectedColor();
        }
        public Image GetSelectImage()
        {
            return selectImage;
        }
        public virtual Color GetDeselectedColor()
        {
            return _deselectedColor;
        }
        protected virtual Color GetSelectedColor()
        {
            return _selectedColor;
        }
    }
}