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
	[SerializeField] private PieceSpawner[] SpawnersInLevel;

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
	    yield return new WaitUntil(() => AudioManager.Instance != null && AudioManager.Instance.IsInitialized);
	    
	    if (musicController) musicController.StartMusic();
	    
	    yield return base.InitializingRoutine();
    }

    public void StartGame()
    {
        HasStarted = true;
        
        if (musicController) musicController.SetMuffle(false);

        if (PuzzlePrefabLevels.Length > 0)
        {
	        Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position, PuzzleSpawnLocation.rotation)
		        .PreparePuzzle();
	        
	        foreach (var spawner in SpawnersInLevel)
	        {
		        spawner.StartSpawning(PuzzlePrefabLevels[currentLevel].Pieces);
	        }
        }
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
		
		//Speed up conveyor belts
		foreach (var conv in ConveyersInLevel)
		{
			conv.SpeedUp();
		}
		
		//Create a new puzzle
		Instantiate(PuzzlePrefabLevels[currentLevel], PuzzleSpawnLocation.position, PuzzleSpawnLocation.rotation).PreparePuzzle();
	}

	//Ends the level
	public void EndGame(bool hasWon)
	{
		SpeedToEnd();
		
		HasStarted = false;
		tries = 3;
		foreach(var spawner in SpawnersInLevel)
		{
			spawner.StopSpawning();
		}
			
		if (musicController) musicController.EndMusic();

		if (!menuController) return;
		if (hasWon) menuController.ShowWinState();
		else menuController.ShowLoseState();
	}
}
