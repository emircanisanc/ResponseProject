using UnityEngine;

public class PhaseControlBtn : MonoBehaviour
{
    void OnEnable()
    {
        if (FindAnyObjectByType<PhaseSequencer>().isEnd)
        {
            gameObject.SetActive(false);
        }


    }
}
