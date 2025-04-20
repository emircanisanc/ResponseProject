using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class Phase2Manager : PhaseSequencer
{
    public Transform leftObjectsParent;
    public Transform rightObjectsParent;

    public float objShowDuration = 5f;

    public int currentObj = 0;

    private List<int> leftObjCallOrder;
    private List<int> rightObjCallOrder;

    public AudioClip heyLookAtClip;
    Coroutine cor;


    protected void ShowCurrentObject(float waitTime = 0)
    {
        if (animatorActive && waitTime == 0) girl.GetComponentInChildren<Animator>().SetTrigger("Idle");

        if (cor != null) StopCoroutine(cor);
        DOTween.Kill(transform);
        HideAllObjects();

        // Handle phase transitions
        if (currentPhase == 0 && currentObj >= leftObjCallOrder.Count)
        {
            EndPhase();
            return;
        }

        if (currentPhase == 0 && leftObjectsParent != null && rightObjectsParent != null)
        {
            int leftIndex = leftObjCallOrder[currentObj];
            int rightIndex = rightObjCallOrder[currentObj];

            leftObjectsParent.GetChild(leftIndex).GetComponent<PhaseObject>()?.SetActive(true);
            rightObjectsParent.GetChild(rightIndex).GetComponent<PhaseObject>()?.SetActive(true);

            cor = StartCoroutine(FivePhase());

            IEnumerator FivePhase()
            {
                yield return new WaitForSeconds(waitTime);
                 yield return new WaitForSeconds(1.5f);
                bool sideLeft = Random.Range(0, 2) == 0;
                string sideAnim = sideLeft ? "L" : "R";
                if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("LookWithEye" + sideAnim);

                yield return new WaitForSeconds(objShowDuration);

                if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("LookWithHead" + sideAnim);
                if (heyLook) AudioSource.PlayClipAtPoint(heyLook, Camera.main.transform.position, audioVol);

                yield return new WaitForSeconds(objShowDuration);

                if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("LookWithBody" + sideAnim);
                if (heyLookAtClip) AudioSource.PlayClipAtPoint(heyLookAtClip, Camera.main.transform.position, audioVol);

                yield return new WaitForSeconds(objShowDuration);

                if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Idle");

                yield return new WaitForSeconds(0.6f);

                if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("LookWithBody" + sideAnim);
                if (heyLookAtClip) AudioSource.PlayClipAtPoint(heyLookAtClip, Camera.main.transform.position, audioVol);

                if (sideLeft) leftObjectsParent.GetChild(leftIndex).GetComponent<PhaseObject>()?.PlayAnim();
                if (!sideLeft) rightObjectsParent.GetChild(rightIndex).GetComponent<PhaseObject>()?.PlayAnim();

                yield return new WaitForSeconds(objShowDuration);

                /* if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("LookWithBody" + sideAnim); */
                currentObj++;
                ShowCurrentObject();

            }
        }

        /* transform.DOScale(transform.localScale, objShowDuration).OnComplete(() =>
        {
            currentObj++;
            ShowCurrentObject();
        }); */
    }

    protected void HideAllObjects()
    {

        foreach (Transform child in leftObjectsParent)
            child.GetComponent<PhaseObject>()?.SetActive(false, true);

        foreach (Transform child in rightObjectsParent)
            child.GetComponent<PhaseObject>()?.SetActive(false, true);
    }

    protected void GoNextObject()
    {
        if (currentPhase == 1 && currentObj >= leftObjCallOrder.Count - 1) return;

        currentObj++;
        ShowCurrentObject();
    }

    protected void GoPreviousObject()
    {
        if (currentObj <= 0)
        {
            if (currentPhase == 0) return;

            return;
        }

        currentObj--;
        ShowCurrentObject();
    }

    public override void ContinuePhase()
    {
        if (!isCutsceneEnd) return;
        if (isEnd || !isStarted) return;
        if (!isStopped) return;
        isStopped = false;
    }

    public Transform girl;
    public Transform girlMovePointsParent;
    public Transform sitPoint;
    public AudioClip firstAudioClip;
    public float headWaitTime = 1.5f;
    public float firstClipTime = 3f;

    public AudioClip heyLook;
    public bool animatorActive = false;

    public float sitTime = 1f;

    protected bool isCutsceneEnd = false;
    public float audioVol = 0.5f;
    public float moveTime = 3f;

    public Transform cam;
    public Transform camPoint;

    public override void StartPhase()
    {
        if (isStarted) return;

        isStarted = true;

        leftObjCallOrder = GenerateRandomOrder(leftObjectsParent.childCount);
        rightObjCallOrder = new List<int>(leftObjCallOrder);

        StartCoroutine(StartingCoroutine());

        IEnumerator StartingCoroutine()
        {

            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Walk");

            cam.DOMove(camPoint.position, moveTime).SetDelay(1f).SetEase(Ease.Linear);

            girl.DOMove(girlMovePointsParent.position, moveTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(moveTime);

            girl.DORotate(girlMovePointsParent.eulerAngles, 0.5f);

            // WAIT TILL GIRL MOVEMENT END
            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Idle");

            girl.parent = sitPoint;

            yield return new WaitForSeconds(1f);

            girl.DOLocalMove(Vector3.zero, sitTime);
            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Sit");

            yield return new WaitForSeconds(sitTime + 1f);
            if (firstAudioClip) AudioSource.PlayClipAtPoint(firstAudioClip, Camera.main.transform.position, audioVol);

            if (animatorActive) girl.GetComponentInChildren<Animator>().SetLayerWeight(1, 1);

            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Talk");

            yield return new WaitForSeconds(firstClipTime + 0.5f);

            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("NodDown");

            ShowCurrentObject(headWaitTime + 1f);
            yield return new WaitForSeconds(headWaitTime);

            if (animatorActive) girl.GetComponentInChildren<Animator>().SetTrigger("Idle");

            isCutsceneEnd = true;

            yield return new WaitForSeconds(headWaitTime);

            
        }

    }

    public override void StopPhase()
    {
        if (!isCutsceneEnd) return;
        if (isEnd || !isStarted) return;
        if (isStopped) return;
        isStopped = true;
    }

    public override void TryGoNext()
    {
        if (!isCutsceneEnd) return;
        if (isEnd || !isStarted) return;
        GoNextObject();
    }

    public override void TryGoPrevious()
    {
        if (!isCutsceneEnd) return;
        if (isEnd || !isStarted) return;
        GoPreviousObject();
    }

    public override void EndPhase()
    {
        if (!isCutsceneEnd) return;
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
