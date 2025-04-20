using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Phase1Manager : PhaseSequencer
{
    public Transform singleObjectsParent;
    public Transform leftObjectsParent;
    public Transform rightObjectsParent;

    public float objShowDuration = 10f;

    public int currentObj = 0;

    private List<int> singleObjCallOrder;
    private List<int> leftObjCallOrder;
    private List<int> rightObjCallOrder;

    protected void ShowCurrentObject()
    {
        DOTween.Kill(transform);
        HideAllObjects(false);

        transform.DOScale(transform.localScale, PhaseObject.DO_CLOSE_TIME).OnComplete(() =>
        {
            if (currentPhase == 0 && currentObj >= singleObjCallOrder.Count)
            {
                currentObj = 0;
                currentPhase = 1;
            }
            else if (currentPhase == 1 && currentObj >= leftObjCallOrder.Count)
            {
                EndPhase();
                return;
            }

            // Show objects based on current phase and call order
            if (currentPhase == 0 && singleObjectsParent != null)
            {
                int index = singleObjCallOrder[currentObj];
                singleObjectsParent.GetChild(index).GetComponent<PhaseObject>()?.SetActive(true);
            }
            else if (currentPhase == 1 && leftObjectsParent != null && rightObjectsParent != null)
            {
                int leftIndex = leftObjCallOrder[currentObj];
                int rightIndex = rightObjCallOrder[currentObj];

                leftObjectsParent.GetChild(leftIndex).GetComponent<PhaseObject>()?.SetActive(true);
                rightObjectsParent.GetChild(rightIndex).GetComponent<PhaseObject>()?.SetActive(true);
            }

            transform.DOScale(transform.localScale, objShowDuration).OnComplete(() =>
            {
                currentObj++;
                ShowCurrentObject();
            });
        });

        // Handle phase transitions

    }

    protected void HideAllObjects(bool instant = true)
    {
        foreach (Transform child in singleObjectsParent)
            child.GetComponent<PhaseObject>()?.SetActive(false, instant);

        foreach (Transform child in leftObjectsParent)
            child.GetComponent<PhaseObject>()?.SetActive(false, instant);

        foreach (Transform child in rightObjectsParent)
            child.GetComponent<PhaseObject>()?.SetActive(false, instant);
    }

    protected void GoNextObject()
    {
        if (currentPhase == 1 && currentObj >= leftObjCallOrder.Count - 1) return;

        currentObj++;
        /* HideAllObjects(true); */
        ShowCurrentObject();
    }

    protected void GoPreviousObject()
    {
        if (currentObj <= 0)
        {
            if (currentPhase == 0) return;

            currentObj = singleObjCallOrder.Count - 1;
            currentPhase--;
            /* HideAllObjects(true); */
            ShowCurrentObject();
            return;
        }

        currentObj--;
        ShowCurrentObject();
    }

    public override void ContinuePhase()
    {
        if (isEnd || !isStarted) return;
        if (!isStopped) return;
        isStopped = false;
    }

    public override void StartPhase()
    {
        if (isStarted) return;

        isStarted = true;

        // Generate randomized call orders
        singleObjCallOrder = GenerateRandomOrder(singleObjectsParent.childCount);
        leftObjCallOrder = GenerateRandomOrder(leftObjectsParent.childCount);
        rightObjCallOrder = new List<int>(leftObjCallOrder);

        ShowCurrentObject();
    }

    public override void StopPhase()
    {
        if (isEnd || !isStarted) return;
        if (isStopped) return;
        isStopped = true;
    }

    public override void TryGoNext()
    {
        if (isEnd || !isStarted) return;
        GoNextObject();
    }

    public override void TryGoPrevious()
    {
        if (isEnd || !isStarted) return;
        GoPreviousObject();
    }

    public override void EndPhase()
    {
        if (isEnd) return;
        isEnd = true;
        OnPhaseEnd?.Invoke();
    }

    private List<int> GenerateRandomOrder(int count)
    {
        List<int> order = new List<int>();
        for (int i = 0; i < count; i++) order.Add(i);

        // Fisher-Yates shuffle
        for (int i = count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (order[i], order[rand]) = (order[rand], order[i]);
        }

        return order;
    }
}
