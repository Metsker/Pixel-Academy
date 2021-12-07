using UnityEngine;

namespace _Scripts.SharedOverall.Tools
{
    public static class ImageAdjuster
    {
        public static void Adjust(RectTransform refRect, Sprite sprite, Vector2 startDelta)
        {
            if (refRect.sizeDelta != startDelta)
            {
                refRect.sizeDelta = startDelta;
            }
            
            float sWidth;
            float sHeight;
            
            var rect = refRect.rect;
            var newWidth = rect.height / sprite.rect.height * sprite.rect.width;
            var newHeight = rect.width / sprite.rect.width * sprite.rect.height;
            
            if (newWidth > rect.width)
            {
                sHeight = newHeight;
                sWidth = rect.width;
            }
            else
            {
                sWidth = newWidth;
                sHeight = rect.height;
            }
            refRect.sizeDelta = new Vector2(sWidth, sHeight);
        }
        
        public static void Adjust(RectTransform refRect, Sprite sprite)
        {
            float sWidth;
            float sHeight;
            
            var rect = refRect.rect;
            var newWidth = rect.height / sprite.rect.height * sprite.rect.width;
            var newHeight = rect.width / sprite.rect.width * sprite.rect.height;
            
            if (newWidth > rect.width)
            {
                sHeight = newHeight;
                sWidth = rect.width;
            }
            else
            {
                sWidth = newWidth;
                sHeight = rect.height;
            }
            refRect.sizeDelta = new Vector2(sWidth, sHeight);
        }
    }
}
