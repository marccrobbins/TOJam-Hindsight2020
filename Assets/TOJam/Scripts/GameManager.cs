using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [EventRef] public string musicEvent;
    
    public bool HasStarted { get; private set; }

	[SerializeField] private Transform PuzzleSpawnLocation;
	[SerializeField] private Puzzle[] PuzzlePrefabLevels;
	[SerializeField] private Conveyer[] ConveyersInLevel;
	[SerializeField] private PieceSpawner[] SpawnersInLevel;

	private int currentLevel = 0;
	private int tries = 3;
    
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
                {"Muffle", 0}
            }
        });
        
		Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position, PuzzleSpawnLocation.rotation).PreparePuzzle();
		foreach(PieceSpawner spawner in SpawnersInLevel)
		{
			spawner.StartSpawning(PuzzlePrefabLevels[currentLevel].Pieces);
		}
    }

	public void SpeedToEnd()
	{
		foreach(Conveyer conv in ConveyersInLevel)
		{
			conv.SpeedUp();
		}
	}

	public void PuzzleDone(bool hasWon)
	{
		if(hasWon)
		{
			Debug.Log("YOU WIN!");
		}
		else
		{
			tries--;
			if(tries <= 0)
			{
				Debug.Log("YOU LOSE");
				tries = 3;
			}
			else
			{
				Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position, PuzzleSpawnLocation.rotation);
			}
		}
		
		AudioManager.Instance.ChangePersistentAudio(new AudioData
		{
			AudioEventName = musicEvent,
			ParameterCollection = new Dictionary<string, float>
			{
				{"End", 1}
			}
		});
	}

}
