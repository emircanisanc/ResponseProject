using DG.Tweening;
using UnityEngine;

public class PhaseObject : MonoBehaviour
{
    public float targetScale = 1f;

    public const float DO_APPEAR_TIME = 0.3f;
    public const float DO_CLOSE_TIME = 0.2f;

    void Awake()
    {
        SetActive(false, true);
    }

    public void PlayAnim()
    {
        float loopDuration = 5f;
        float singleAnimTime = loopDuration / 4f; // Half for scale up, half for scale down

        transform.localScale = Vector3.one * targetScale; // Ensure starting scale

        transform.DOScale(targetScale * 1.2f, singleAnimTime)
            .SetLoops(4, LoopType.Yoyo) // Scale up, then scale back
            .SetEase(Ease.InOutSine);
    }


    public void StopAnim()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector3.one * targetScale;
    }

    public void SetActive(bool isActive, bool instant = false)
    {
        DOTween.Kill(transform);
        if (isActive)
        {
            if (!instant)
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(targetScale, DO_APPEAR_TIME);
            }
            else
            {
                transform.localScale = Vector3.one * targetScale;
            }
        }
        else
        {
            StopAnim();
            if (!instant)
            {
                transform.localScale = Vector3.one * targetScale;
                transform.DOScale(0f, DO_CLOSE_TIME);
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
    }
}
