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

    protected Vector3 startScale;

    private void Awake() {
        startScale = transform.localScale;
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
}
