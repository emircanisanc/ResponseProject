using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class CardObject : MonoBehaviour
{
    [SerializeField] protected Image[] icons;
    [SerializeField] protected CanvasGroup canvasGroup;

    public int IconCount => icons.Length;

    protected Vector3 startScale;
    protected Vector3 startPos;

    private void Awake()
    {
        startScale = transform.localScale;
        startPos = transform.position;
    }

    public void ShowCard()
    {
        canvasGroup.DOFade(1f, 0.3f);
    }

    public void ShowInstant()
    {
        canvasGroup.alpha = 1;
    }

    public void CloseCard()
    {
        canvasGroup.DOFade(0f, 0.3f);
    }

    public void CloseCardInstant()
    {
        canvasGroup.alpha = 0;
    }

    public void ScaleCard()
    {
        transform.DOScale(startScale * 1.1f, 0.3f);
    }

    public void ScaleNormal()
    {
        transform.DOScale(startScale, 0.3f);
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

    IEnumerator AnimationCoroutine()
    {
        // Clear any active animations
        yield return new WaitForSeconds(0.5f);
        transform.DOKill();

        Sequence seq = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();

        // Scale up and down
        seq.Append(transform.DOScale(startScale * 1.25f, 0.5f).SetEase(Ease.Linear))
           .Append(transform.DOScale(startScale, 0.5f).SetEase(Ease.Linear))
           .SetLoops(-1, LoopType.Yoyo); // -1 for infinite loop

        // Move up and down
        float delta = 50f;
        /* seq2.Append(transform.DOMoveY(startPos.y + delta, 0.25f).SetEase(Ease.Linear))
           .Append(transform.DOMoveY(startPos.y, 0.25f).SetEase(Ease.Linear))
           .Append(transform.DOMoveY(startPos.y - delta, 0.25f).SetEase(Ease.Linear))
           .Append(transform.DOMoveY(startPos.y, 0.25f).SetEase(Ease.Linear))
           .SetLoops(-1, LoopType.Yoyo); // -1 for infinite loop */

        yield return new WaitForSeconds(5f);

        // Stop the animations after 5 seconds
        seq.Kill();

        ScaleNormal();
        transform.DOMoveY(startPos.y, 0.2f);
    }

}
