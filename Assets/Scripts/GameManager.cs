using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public Transform[] cardParents;

    [SerializeField] protected CardObject cardLeft;
    [SerializeField] protected CardObject cardRight;

    public Transform[] cameraDefaultPoses;
    public Transform[] cameraZoomPoses;
    public Transform cameraZoomRightPos;
    public Transform cameraZoomLeftPos;

    public Transform cam;

    protected int lastSelected = 20;
    public int stageCounter = 0;

    public CanvasGroup blackCanvas;

    public Animator woman;

    bool isComplete;
    bool isStarted = true;

    public float DelayAfterCardsClosed = 1.5f;

    public Transform rightCloseUp;
    public Transform leftCloseUp;

    private void Start()
    {
        Invoke(nameof(StartAllStages), 0.2f);
    }

    private void Update()
    {
        if (isStarted)
        {
            if (isComplete)
            {
                isComplete = false;
                stageCounter++;
                switch (stageCounter)
                {
                    case 1:
                        StartCoroutine(Stage1());
                        break;
                    case 2:
                        StartCoroutine(Stage2());
                        break;
                    case 3:
                        StartCoroutine(Stage3());
                        break;
                    case 4:
                        EditorApplication.isPlaying = false;
                        break;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Time.timeScale = 5;
        }

    }

    public bool StopAfterStageEnd;

    private void StartAllStages()
    {
        switch (stageCounter)
        {
            case 0:
                StartCoroutine(Stage0());
                break;
            case 1:
                StartCoroutine(Stage1());
                break;
            case 2:
                StartCoroutine(Stage2());
                break;
            case 3:
                StartCoroutine(Stage3());
                break;
        }
    }

    IEnumerator Stage0()
    {
        cardRight = cardParents[stageCounter].GetChild(0).GetComponent<CardObject>();
        cardLeft = cardParents[stageCounter].GetChild(1).GetComponent<CardObject>();
        woman.SetTrigger("Idle");

        cam.position = cameraDefaultPoses[stageCounter].position;

        yield return new WaitForSeconds(5f);

        for (int counter = 0; counter < 5; counter++)
        {
            int selection = Random.Range(0, cardLeft.IconCount);
            while (selection == lastSelected || selection % 2 != 0)
            {
                selection = Random.Range(0, cardLeft.IconCount);
                yield return null;
            }

            lastSelected = selection;

            cardLeft.SetImage(selection);
            cardRight.SetImage(selection + 1);

            cardLeft.ShowCard();
            cardRight.ShowCard();

            yield return new WaitForSeconds(durationOfAlpha);

            string triggerName = "Look";

            bool isLeft = Random.Range(0, 2) == 0;

            triggerName += isLeft ? "L" : "R";

            woman.SetTrigger(triggerName);

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (i % 2 == 0)
                    woman.SetTrigger("Idle");
                else
                    woman.SetTrigger(triggerName);
            }

            cardLeft.CloseCard();
            cardRight.CloseCard();

            yield return new WaitForSeconds(DelayAfterCardsClosed);
        }

        if (StopAfterStageEnd)
            EditorApplication.isPlaying = false;

        blackCanvas.DOFade(1f, blackCanvasDuration);

        yield return new WaitForSeconds(1f);

        cam.transform.position = cameraDefaultPoses[stageCounter + 1].position;

        blackCanvas.DOFade(0f, blackCanvasDuration).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    IEnumerator Stage1()
    {
        cardRight = cardParents[stageCounter].GetChild(0).GetComponent<CardObject>();
        cardLeft = cardParents[stageCounter].GetChild(1).GetComponent<CardObject>();
        woman.SetTrigger("Idle");

        cam.transform.position = cameraDefaultPoses[stageCounter].position;

        yield return new WaitForSeconds(5f);

        for (int counter = 0; counter < 5; counter++)
        {
            int selection = Random.Range(0, cardLeft.IconCount);
            while (selection == lastSelected || selection % 2 != 0)
            {
                selection = Random.Range(0, cardLeft.IconCount);
                yield return null;
            }

            lastSelected = selection;

            cardLeft.SetImage(selection);
            cardRight.SetImage(selection + 1);

            cardLeft.ShowCard();
            cardRight.ShowCard();

            yield return new WaitForSeconds(durationOfAlpha);

            cam.transform.DOMove(cameraZoomPoses[1].position, durationOfAlpha);

            yield return new WaitForSeconds(durationOfAlpha);

            woman.SetTrigger("HeyLook");

            yield return new WaitForSeconds(2f);

            cam.transform.DOMove(cameraDefaultPoses[stageCounter].position, durationOfResetCam);

            woman.SetTrigger("Idle");

            string triggerName = "Turn";

            bool isLeft = Random.Range(0, 2) == 0;

            triggerName += isLeft ? "L" : "R";

            woman.SetTrigger(triggerName);

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (i % 2 == 0)
                    woman.SetTrigger("Idle");
                else
                    woman.SetTrigger(triggerName);
            }

            cardLeft.CloseCard();
            cardRight.CloseCard();

            yield return new WaitForSeconds(DelayAfterCardsClosed);
        }

        if (StopAfterStageEnd)
            EditorApplication.isPlaying = false;

        blackCanvas.DOFade(1f, blackCanvasDuration);

        yield return new WaitForSeconds(1f);

        cam.transform.position = cameraDefaultPoses[stageCounter + 1].position;

        blackCanvas.DOFade(0f, blackCanvasDuration).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    protected float durationOfAlpha = 1.2f;
    protected float durationOfResetCam = 0.8f;

    protected float leftAngle = 210;
    protected float rightAngle = 130;
    protected float turnSideDuration = 0.8f;
    protected float turnNormalDuration = 0.8f;
    protected float defaultAngle = 180;

    public void ChangeRotation(bool isLeft, bool focus = false)
    {
        if (focus)
            ChangeCardPo(isLeft);
        StartCoroutine(DelayBeforeRotate());
        IEnumerator DelayBeforeRotate()
        {
            yield return new WaitForSeconds(0.3f);
            if (isLeft)
                woman.transform.DORotate(new Vector3(0, leftAngle, 0), turnSideDuration);
            else
                woman.transform.DORotate(new Vector3(0, rightAngle, 0), turnSideDuration);
        }

    }

    public void ChangeCardPo(bool isLeft)
    {
        StartCoroutine(DelayBeforeRotate());
        IEnumerator DelayBeforeRotate()
        {
            yield return new WaitForSeconds(0);
            /* yield return new WaitForSeconds(durationOfAlpha); */
            if (isLeft)
            {
                cardLeft.transform.DOMove(leftCloseUp.transform.GetChild(1).position, turnSideDuration);
                cardRight.transform.DOMove(leftCloseUp.transform.GetChild(0).position, turnSideDuration);
            }
            else
            {
                cardLeft.transform.DOMove(rightCloseUp.transform.GetChild(1).position, turnSideDuration);
                cardRight.transform.DOMove(rightCloseUp.transform.GetChild(0).position, turnSideDuration);
            }
        }

    }

    IEnumerator Stage2()
    {
        cardRight = cardParents[stageCounter].GetChild(0).GetComponent<CardObject>();
        cardLeft = cardParents[stageCounter].GetChild(1).GetComponent<CardObject>();
        woman.SetTrigger("Idle");

        cam.transform.position = cameraDefaultPoses[stageCounter].position;

        yield return new WaitForSeconds(5f);

        for (int counter = 0; counter < 5; counter++)
        {
            int selection = Random.Range(0, cardLeft.IconCount);
            while (selection == lastSelected || selection % 2 != 0)
            {
                selection = Random.Range(0, cardLeft.IconCount);
                yield return null;
            }

            lastSelected = selection;

            cardLeft.SetImage(selection);
            cardRight.SetImage(selection + 1);

            cardLeft.ShowCard();
            cardRight.ShowCard();

            yield return new WaitForSeconds(durationOfAlpha);

            cam.transform.DOMove(cameraZoomPoses[2].position, durationOfAlpha);

            yield return new WaitForSeconds(durationOfAlpha);

            woman.SetTrigger("HeyLookThis");

            yield return new WaitForSeconds(2f);

            cam.transform.DOMove(cameraDefaultPoses[stageCounter].position, durationOfResetCam);

            woman.SetTrigger("Idle");

            string triggerName = "Point";

            bool isLeft = Random.Range(0, 2) == 0;

            triggerName += isLeft ? "L" : "R";

            woman.SetTrigger(triggerName);

            ChangeRotation(isLeft);

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (i % 2 == 0)
                {
                    woman.SetTrigger("Idle");
                    woman.transform.DORotate(new Vector3(0, defaultAngle, 0), turnNormalDuration);
                    cardLeft.ResetPos();
                    cardRight.ResetPos();
                }
                else
                {
                    ChangeRotation(isLeft);
                    woman.SetTrigger(triggerName);
                }
            }

            cardLeft.CloseCard();
            cardRight.CloseCard();

            yield return new WaitForSeconds(DelayAfterCardsClosed);
        }

        if (StopAfterStageEnd)
            EditorApplication.isPlaying = false;

        blackCanvas.DOFade(1f, blackCanvasDuration);

        yield return new WaitForSeconds(1f);

        cam.transform.position = cameraDefaultPoses[stageCounter + 1].position;

        blackCanvas.DOFade(0f, blackCanvasDuration).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    IEnumerator Stage3()
    {
        cardRight = cardParents[stageCounter].GetChild(0).GetComponent<CardObject>();
        cardLeft = cardParents[stageCounter].GetChild(1).GetComponent<CardObject>();
        woman.SetTrigger("Idle");

        cam.transform.position = cameraDefaultPoses[stageCounter].position;

        yield return new WaitForSeconds(5f);

        for (int counter = 0; counter < 5; counter++)
        {
            int selection = Random.Range(0, cardLeft.IconCount);
            while (selection == lastSelected || selection % 2 != 0)
            {
                selection = Random.Range(0, cardLeft.IconCount);
                yield return null;
            }

            lastSelected = selection;

            cardLeft.SetImage(selection);
            cardRight.SetImage(selection + 1);

            cardLeft.ShowCard();
            cardRight.ShowCard();

            yield return new WaitForSeconds(durationOfAlpha);

            cam.transform.DOMove(cameraZoomPoses[3].position, durationOfAlpha);

            yield return new WaitForSeconds(durationOfAlpha);

            woman.SetTrigger("HeyLookThis");

            yield return new WaitForSeconds(2f);

            cam.transform.DOMove(cameraDefaultPoses[stageCounter].position, durationOfResetCam);

            woman.SetTrigger("Idle");

            string triggerName = "Point";

            bool isLeft = Random.Range(0, 2) == 0;

            triggerName += isLeft ? "L" : "R";

            woman.SetTrigger(triggerName);

            ChangeRotation(isLeft, true);
            /* ChangeCardPo(isLeft); */

            if (isLeft)
            {
                cardLeft.PlayFocusAnim();
                cam.transform.DOMove(cameraZoomLeftPos.position, durationOfAlpha);
            }
            else
            {
                cam.transform.DOMove(cameraZoomRightPos.position, durationOfAlpha);
                cardRight.PlayFocusAnim();
            }

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (i % 2 == 0)
                {
                    woman.SetTrigger("Idle");
                    cam.transform.DOMove(cameraDefaultPoses[stageCounter].position, durationOfAlpha);
                    woman.transform.DORotate(new Vector3(0, defaultAngle, 0), turnNormalDuration);
                    cardLeft.ResetPos();
                    cardRight.ResetPos();
                }
                else
                {
                    woman.SetTrigger(triggerName);
                    ChangeRotation(isLeft, true);
                    /* ChangeCardPo(isLeft); */
                    if (isLeft)
                    {
                        cardLeft.PlayFocusAnim();
                        cam.transform.DOMove(cameraZoomLeftPos.position, durationOfAlpha);
                    }
                    else
                    {
                        cam.transform.DOMove(cameraZoomRightPos.position, durationOfAlpha);
                        cardRight.PlayFocusAnim();
                    }
                }
            }

            cardLeft.CloseCard();
            cardRight.CloseCard();

            yield return new WaitForSeconds(DelayAfterCardsClosed);
        }

        if (StopAfterStageEnd)
            EditorApplication.isPlaying = false;

        blackCanvas.DOFade(1f, blackCanvasDuration);

        yield return new WaitForSeconds(1f);

        blackCanvas.DOFade(0f, blackCanvasDuration).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    protected float blackCanvasDuration = 0.7f;
}
