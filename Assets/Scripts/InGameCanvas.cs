using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCanvas : MonoBehaviour
{
    bool isPaused = false;
    bool isLoading = false;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    public CanvasGroup canvasGroup;

    void Awake()
    {
        FindAnyObjectByType<PhaseSequencer>().OnPhaseEnd += TogglePause;
        canvasGroup.interactable = false;
    }

    void Update()
    {
        if (isLoading) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindAnyObjectByType<PhaseSequencer>().TryGoNext();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindAnyObjectByType<PhaseSequencer>().TryGoPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            /* FindAnyObjectByType<PhaseSequencer>().StartPhase(); */
        }
    }

    void Start()
    {
        transform.DOScale(transform.localScale, 0.1f).OnComplete(() => {
            FindAnyObjectByType<PhaseSequencer>().StartPhase();
        });
    }

    public void ReturnToMenu()
    {
        if (isLoading) return;

        Time.timeScale = 1f;
        isLoading = true;
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        if (isLoading) return;

        Time.timeScale = 1f;
        isLoading = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause(bool active)
    {
        if (isLoading) return;
        isPaused = active;
        canvasGroup.DOKill();
        if (isPaused)
        {
            canvasGroup.DOFade(1f, fadeInDuration).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                Time.timeScale = 0.001f;
            });
        }
        else
        {
            canvasGroup.interactable = false;
            Time.timeScale = 1f;
            canvasGroup.DOFade(0f, fadeOutDuration);
        }
    }

    public void TogglePause()
    {
        TogglePause(!isPaused);
    }
}
