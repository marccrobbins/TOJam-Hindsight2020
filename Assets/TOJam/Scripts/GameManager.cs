using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Rewired;
using UnityEditor.EventSystems;
using UnityEngine;

public class GameManager : Manager
{
    public static GameManager Instance;

    public bool HasStarted { get; private set; }

    [SerializeField] private MusicController musicController;
    [SerializeField] private MenuController menuController;
	[SerializeField] private Transform PuzzleSpawnLocation;
	[SerializeField] private Puzzle[] PuzzlePrefabLevels;
	[SerializeField] private Conveyer[] ConveyersInLevel;

	private Puzzle activePuzzle;
	private int currentLevel = 0;
	private int tries = 3;

	protected override void Initialize()
	{
        DontDestroyOnLoad(gameObject);
        
        Instance = this;

        base.Initialize();
    }

    protected override IEnumerator InitializingRoutine()
    {
	    yield return new WaitUntil(() => AudioManager.Instance != null && 
	                                     AudioManager.Instance.IsInitialized);
	    
	    if (musicController) musicController.StartMusic();
	    
	    yield return base.InitializingRoutine();
    }

    public void StartGame()
    {
	    ResetGame();

	    HasStarted = true;

	    if (musicController) musicController.SetMuffle(false);
	   
	    if (PuzzlePrefabLevels.Length == 0) return;
	    
	    //Prepare puzzle
	    activePuzzle = Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position,
		    PuzzleSpawnLocation.rotation);
	    activePuzzle.PreparePuzzle();

	    //Start spawning
	    SpawnManager.Instance.StartSpawning();
    }

    public void SpeedToEnd()
	{
		foreach (var conv in ConveyersInLevel)
		{
			conv.SpeedUp();
		}
	}

	//Create a new puzzle if there are more tries available or fail the level
	public void TryAgain()
	{
		tries--;
		if (tries <= 0)
		{
			EndGame(false);
			return;
		}
		
		//Create a new puzzle
		activePuzzle = Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position, PuzzleSpawnLocation.rotation);
		activePuzzle.PreparePuzzle();
	}

	//Ends the level
	public void EndGame(bool hasWon)
	{
		SpeedToEnd();
		
		HasStarted = false;
		tries = 3;
		SpawnManager.Instance.StopSpawning();
			
		if (musicController) musicController.EndMusic();
		
		if (!menuController) return;
		
		if (hasWon) menuController.ShowWinState();
		else menuController.ShowLoseState();
	}

	public void ResetGame()
	{
		if (!HasStarted) return;

		HasStarted = false;
		Time.timeScale = 1;
		tries = 3;
		
		PuzzlePrefabLevels[currentLevel].ResetPuzzle();
		if (activePuzzle) Destroy(activePuzzle.gameObject);

		foreach (var conveyor in ConveyersInLevel)
		{
			if (!conveyor) continue;
			conveyor.SlowDown();
		}

		SpawnManager.Instance.ResetSpawner();
	}
}
