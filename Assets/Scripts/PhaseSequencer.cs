using System;
using UnityEngine;

public abstract class PhaseSequencer : MonoBehaviour
{
    public int currentPhase = 0;
    public Action OnPhaseEnd;
    public Action OnPhaseStarted;
    public Action OnPhaseContinue;
    protected bool isStarted = false;
    public bool isEnd = false;
    protected bool isStopped = false;
    public abstract void StartPhase();
    public abstract void StopPhase();
    public abstract void ContinuePhase();
    public abstract void TryGoPrevious();
    public abstract void TryGoNext();
    public abstract void EndPhase();
}
