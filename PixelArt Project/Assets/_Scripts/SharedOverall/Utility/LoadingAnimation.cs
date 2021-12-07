using DG.Tweening;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    public void Animate()
    {
        transform.DORotate(new Vector3(0, 0, -360), 3).SetRelative()
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void ToggleState(bool state)
    {
        gameObject.SetActive(state);
    }
}