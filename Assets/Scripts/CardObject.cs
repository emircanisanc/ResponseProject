using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardObject : MonoBehaviour
{
    [SerializeField] protected Image[] icons;
    [SerializeField] protected CanvasGroup canvasGroup;

    public int IconCount => icons.Length;

    protected Vector3 startScale;
    protected Vector3 startPos;

    public MeshRenderer[] meshRenderers;

    public GameObject outlineObj;

    private void Awake()
    {
        startScale = transform.localScale;
        startPos = transform.position;

        foreach (var renderer in meshRenderers)
        {
            renderer.material = Instantiate(renderer.material);
        }
        outlineObj.SetActive(false);


        CloseCardInstant();
    }

    

    protected float durationOfAlpha = 1.2f;

    public void ShowCard()
    {
        canvasGroup.DOFade(1f, durationOfAlpha);
        foreach (var renderer in meshRenderers)
        {
            Color col = renderer.material.color;
            col.a = 1;
            renderer.material.DOColor(col, durationOfAlpha);
        }
    }

    public void ShowInstant()
    {
        canvasGroup.alpha = 1;
    }

    public void CloseCard()
    {
        canvasGroup.DOFade(0f, durationOfAlpha);
        foreach (var renderer in meshRenderers)
        {
            Color col = renderer.material.color;
            col.a = 0;
            renderer.material.DOColor(col, durationOfAlpha);
        }
    }

    public void CloseCardInstant()
    {
        canvasGroup.alpha = 0;
        foreach (var renderer in meshRenderers)
        {
            Color col = renderer.material.color;
            col.a = 0;
            renderer.material.SetColor("_BaseColor", col);
        }
    }

    public void ScaleCard()
    {
        transform.DOScale(startScale * 1.1f, durationOfAlpha);
    }

    public void ScaleNormal()
    {
        transform.DOScale(startScale, durationOfAlpha);
    }

    public void SetImage(int index)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(index == i);
        }
    }

    public void PlayFocusAnim()
    {
        StartCoroutine(AnimationCoroutine());
    }


    public void ResetPos()
    {
        transform.DOMove(startPos, durationOfAlpha);
    }

    public void PlayNotFocusedAnim()
    {
        /* StartCoroutine(NotFocused());
        IEnumerator NotFocused()
        {
            float startX = transform.position.x;

            transform.DOMoveX(startX / 2, 0.53f);

            yield return new WaitForSeconds(5);

            transform.DOMoveX(startX, 0.5f);
        } */
    }

    IEnumerator AnimationCoroutine()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject != gameObject)
            {
                if (transform.parent.GetChild(i).TryGetComponent<CardObject>(out var card))
                {
                    card.PlayNotFocusedAnim();
                }
            }
        }
        // Clear any active animations
        yield return new WaitForSeconds(0.5f);
        transform.DOKill();

        Sequence seq = DOTween.Sequence();

        outlineObj.SetActive(true);

        // Scale up and down
        seq.Append(transform.DOScale(startScale * 1.25f, 1f).SetEase(Ease.Linear))
           .Append(transform.DOScale(startScale, 1f).SetEase(Ease.Linear))
           .SetLoops(-1, LoopType.Yoyo); // -1 for infinite loop


        yield return new WaitForSeconds(5f);

        // Stop the animations after 5 seconds
        seq.Kill();
        outlineObj.SetActive(false);

        ScaleNormal();
        transform.DOMoveY(startPos.y, 0.2f);
    }

}
