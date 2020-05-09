using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [EventRef] public string musicEvent;
    
    public bool HasStarted { get; private set; }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        Instance = this;
        
        AudioManager.Instance.PlayPersistentAudio(new AudioData
        {
            AudioEventName = musicEvent, 
            ParameterCollection = new Dictionary<string, float>
            {
                {"End", 0},
                {"Muffle", 1}
            }
        });
    }

    public void StartGame()
    {
        HasStarted = true;
        
        AudioManager.Instance.ChangePersistentAudio(new AudioData
        {
            AudioEventName = musicEvent,
            ParameterCollection = new Dictionary<string, float>
            {
                {"End", 0},
                {"Muffle", 0}
            }
        });
    }
}
