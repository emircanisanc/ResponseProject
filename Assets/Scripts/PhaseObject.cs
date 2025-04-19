using DG.Tweening;
using UnityEngine;

public class PhaseObject : MonoBehaviour
{
    public const float DO_APPEAR_TIME = 0.3f;
    public const float DO_CLOSE_TIME = 0.2f;

    void Awake()
    {
        SetActive(false, true);
    }

    public void PlayAnim()
    {

    }

    public void StopAnim()
    {

    }

    public void SetActive(bool isActive, bool instant = false)
    {
        DOTween.Kill(transform);
        if (isActive)
        {
            if (!instant)
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(1f, DO_APPEAR_TIME);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
        else
        {
            StopAnim();
            if (!instant)
            {
                transform.localScale = Vector3.one;
                transform.DOScale(0f, DO_CLOSE_TIME);
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
    }
}
