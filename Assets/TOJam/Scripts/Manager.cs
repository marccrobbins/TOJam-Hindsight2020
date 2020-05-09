using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool IsInitializing { get; protected set; }
    public bool IsInitialized { get; protected set; }
    
    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        StartCoroutine(InitializingRoutine());
    }

    protected virtual IEnumerator InitializingRoutine()
    {
        IsInitializing = true;
        yield return StartCoroutine(InitializedRoutine());
    }

    protected virtual IEnumerator InitializedRoutine()
    {
        IsInitializing = false;
        IsInitialized = true;
        yield break;
    }
}
