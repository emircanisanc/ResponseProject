using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected CardObject cardLeft;
    [SerializeField] protected CardObject cardRight;

    public Transform cameraDefaultPos;
    public Transform cameraZoomPos;
    public Transform cameraZoomRightPos;
    public Transform cameraZoomLeftPos;

    public Transform cam;

    protected int lastSelected = 20;
    public int stageCounter = 0;

    public CanvasGroup blackCanvas;

    public Animator woman;

    bool isComplete;
    bool isStarted = true;

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
        woman.SetTrigger("Idle");

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

            yield return new WaitForSeconds(0.3f);

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

            yield return new WaitForSeconds(1f);
        }

        blackCanvas.DOFade(1f, 0.3f);

        yield return new WaitForSeconds(1f);

        blackCanvas.DOFade(0f, 0.3f).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    IEnumerator Stage1()
    {
        woman.SetTrigger("Idle");

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

            cam.transform.DOMove(cameraZoomPos.position, 0.3f);

            yield return new WaitForSeconds(0.3f);

            woman.SetTrigger("HeyLook");

            yield return new WaitForSeconds(1f);

            cam.transform.DOMove(cameraDefaultPos.position, 0.2f);

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

            yield return new WaitForSeconds(1f);
        }

        blackCanvas.DOFade(1f, 0.3f);

        yield return new WaitForSeconds(1f);

        blackCanvas.DOFade(0f, 0.3f).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    IEnumerator Stage2()
    {
        woman.SetTrigger("Idle");

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

            cam.transform.DOMove(cameraZoomPos.position, 0.3f);

            yield return new WaitForSeconds(0.3f);

            woman.SetTrigger("HeyLookThis");

            yield return new WaitForSeconds(1f);

            cam.transform.DOMove(cameraDefaultPos.position, 0.2f);

            woman.SetTrigger("Idle");

            string triggerName = "Point";

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

            yield return new WaitForSeconds(1f);
        }

        blackCanvas.DOFade(1f, 0.3f);

        yield return new WaitForSeconds(1f);

        blackCanvas.DOFade(0f, 0.3f).OnComplete(() =>
        {
            isComplete = true;
        });
    }

    IEnumerator Stage3()
    {
        woman.SetTrigger("Idle");

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

            cam.transform.DOMove(cameraZoomPos.position, 0.3f);

            yield return new WaitForSeconds(0.3f);

            woman.SetTrigger("HeyLookThis");

            yield return new WaitForSeconds(1f);

            cam.transform.DOMove(cameraDefaultPos.position, 0.2f);

            woman.SetTrigger("Idle");

            string triggerName = "Point";

            bool isLeft = Random.Range(0, 2) == 0;

            triggerName += isLeft ? "L" : "R";

            woman.SetTrigger(triggerName);

            if (isLeft)
            {
                cardLeft.PlayFocusAnim();
                cam.transform.DOMove(cameraZoomLeftPos.position, 0.3f);
            }
            else
            {
                cam.transform.DOMove(cameraZoomRightPos.position, 0.3f);
                cardRight.PlayFocusAnim();
            }

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(5f);
                if (i % 2 == 0)
                {
                    woman.SetTrigger("Idle");
                    cam.transform.DOMove(cameraDefaultPos.position, 0.3f);
                }
                else
                {
                    woman.SetTrigger(triggerName);
                    if (isLeft)
                    {
                        cardLeft.PlayFocusAnim();
                        cam.transform.DOMove(cameraZoomLeftPos.position, 0.3f);
                    }
                    else
                    {
                        cam.transform.DOMove(cameraZoomRightPos.position, 0.3f);
                        cardRight.PlayFocusAnim();
                    }
                }
            }

            cardLeft.CloseCard();
            cardRight.CloseCard();

            yield return new WaitForSeconds(1f);
        }

        blackCanvas.DOFade(1f, 0.3f);

        yield return new WaitForSeconds(1f);

        blackCanvas.DOFade(0f, 0.3f).OnComplete(() =>
        {
            isComplete = true;
        });
    }
}
