using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.SharedOverall.Tools.Logic
{
    public abstract class SelectableTool : BaseTool, ISelectable
    {
        [SerializeField] private Image selectImage;
        private readonly Color _selectedColor = Color.white;
        private readonly Color _deselectedColor = new Vector4(1,1,1,0);
        
        public void Select()
        {
            GetSelectImage().color = GetSelectedColor();
            ToolsManager.DeselectTools(this);
        }
        public void SelectWithAnimation()
        {
            toolAnimation.PlayAnimation();
            GetSelectImage().color = GetSelectedColor();
            ToolsManager.DeselectTools(this);
        }
        public virtual void Deselect()
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