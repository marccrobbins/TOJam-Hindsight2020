using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public bool HasStarted { get; private set; }
    
    private void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        HasStarted = true;
    }
}
