using UnityEngine;

public class PhaseControlBtn : MonoBehaviour
{
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;

        /* if (timer < 1f) return; */

        timer = 0;
        if (FindAnyObjectByType<PhaseSequencer>().isEnd)
        {
            gameObject.SetActive(false);
        }
    }
}
