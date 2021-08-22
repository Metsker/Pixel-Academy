using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic.Tools
{
    public static class ImageAdjuster
    {
        public static void Adjust(RectTransform refRect, Sprite sprite)
        {
            var rect = refRect.rect;

            var sWidth = sprite.rect.width;
            var sHeight = sprite.rect.height;

            var absW = Mathf.Abs(rect.width - sWidth);
            var absH = Mathf.Abs(rect.height - sHeight);
            
            if (absW >= absH)
            {
                sHeight = rect.width / sprite.rect.width * sprite.rect.height;
                sWidth = rect.width;
            }
            else
            {
                sWidth = rect.height / sprite.rect.height * sprite.rect.width;
                sHeight = rect.height;
            }
            
            refRect.sizeDelta = new Vector2(sWidth, sHeight);
        }
    }
}
