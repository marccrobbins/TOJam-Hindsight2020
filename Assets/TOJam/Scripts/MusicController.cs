using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [EventRef] public string musicEvent;

    private AudioData musicData;

    public void StartMusic()
    {
        musicData = new AudioData
        {
            AudioEventName = musicEvent, 
            ParameterCollection = new Dictionary<string, float>
            {
                {"End", 0},
                {"Muffle", 1}
            }
        };
        
        AudioManager.Instance.PlayPersistentAudio(musicData);
    }

    public void SetMuffle(bool isActive)
    {
        musicData.ParameterCollection["Muffle"] = isActive ? 1 : 0;
        AudioManager.Instance.ChangePersistentAudio(musicData);
    }

    public void EndMusic()
    {
        musicData.ParameterCollection["End"] = 1;
        musicData.ParameterCollection["Muffle"] = 0;
        AudioManager.Instance.ChangePersistentAudio(musicData);
    }
}
