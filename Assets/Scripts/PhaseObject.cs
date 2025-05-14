using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;

public class PhaseObject : MonoBehaviour
{
    public float targetScale = 1f;

    public const float DO_APPEAR_TIME = 1f;
    public const float DO_CLOSE_TIME = 0.7f;

    public AudioClip audioClip;

    private Animator animator;
    private AudioSource audioSource;

    [Header("PHASE 3 ANIMATIONS")]
    public bool useMovement = true;
    public Transform startPoint;
    public Transform endPoint;

    public void PlaySound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        var parent = transform.GetChild(0).gameObject;
        new GameObject("source").transform.parent = parent.transform;
        audioSource = parent.transform.GetChild(parent.transform.childCount - 1).gameObject.AddComponent<AudioSource>();
        audioSource.transform.localPosition = new Vector3(transform.position.x * 3, 0, 0);
        audioSource.panStereo = Math.Clamp(transform.position.x, -1, 1);
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        SetActive(false, true);
    }

    public void PlayAnim(bool isHint = true)
    {
        if (isPlayingAnim) return;

        isPlayingAnim = true;

        if (!isHint)
        {
            if (animator) animator.enabled = true;



            return;
        }
        float loopDuration = 5f;
        float singleAnimTime = loopDuration / 4f; // Half for scale up, half for scale down

        transform.localScale = Vector3.one * targetScale; // Ensure starting scale

        /* if (USE_DO_SCALE)
        {

        }
        else
        {

        } */

        transform.DOScale(targetScale * 1.2f, singleAnimTime)
            .SetLoops(4, LoopType.Yoyo) // Scale up, then scale back
            .SetEase(Ease.InOutSine);
    }


    public void StopAnim()
    {
        if (isPlayingAnim)
        {
            isPlayingAnim = false;
            audioSource.Stop();
            DOTween.Kill(transform);
            transform.localScale = Vector3.one * targetScale;


        }

    }

    private bool USE_DO_SCALE = false;

    bool isPlayingAnim = false;

    bool isActiveNow = true;

    public void SetActive(bool isActive, bool instant = false)
    {
        DOTween.Kill(transform);
        if (isActive && !isActiveNow)
        {
            isActiveNow = true;
            if (endPoint && useMovement)
            {
                transform.localScale = Vector3.one * targetScale;
                transform.GetChild(0).localPosition = startPoint.localPosition;
                transform.GetChild(0).DOLocalMove(endPoint.localPosition, 1f).OnComplete(() =>
                {

                });
            }
            if (!instant && !useMovement)
            {
                transform.localScale = Vector3.zero;

                if (USE_DO_SCALE) transform.DOScale(targetScale, DO_APPEAR_TIME);
                else transform.localScale = targetScale * Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.one * targetScale;
            }
        }
        else
        {


            StopAnim();

            if (animator) animator.enabled = false;

            if (!instant && isActiveNow)
            {
                if (USE_DO_SCALE)
                {
                    transform.localScale = Vector3.one * targetScale;
                    transform.DOScale(0f, DO_CLOSE_TIME);
                }
                else
                {
                    transform.localScale = Vector3.zero;
                }

            }
            else
            {
                if (endPoint && useMovement)
                {
                    /* transform.GetChild(0).localPosition = startPoint.localPosition; */
                }
                transform.localScale = Vector3.zero;
            }

            isActiveNow = false;
        }
    }
}
