using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : Manager
{
    public static SpawnManager Instance;

    public Transform leftSpawn, rightSpawn;
    public float spawnDelay;

    private bool canSpawn;
    private float timePassed;
    private Dictionary<SpawnSide, Dictionary<string, PuzzlePiece>> registeredPiecesLookup;
    private List<GameObject> spawnedPieces;
    
    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        
        Instance = this;

        //Create lookup
        registeredPiecesLookup = new Dictionary<SpawnSide, Dictionary<string, PuzzlePiece>>
        {
            {SpawnSide.Left, new Dictionary<string, PuzzlePiece>()},
            {SpawnSide.Right, new Dictionary<string, PuzzlePiece>()}
        };
        
        base.Initialize();
    }

    public void StartSpawning()
    {
        canSpawn = true;
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }

    public void RegisterPuzzlePiece(PuzzlePiece piece, SpawnSide side)
    {
        var registry = registeredPiecesLookup[side];
        if (registry.ContainsKey(piece.identifier)) return;
        registry.Add(piece.identifier, piece);
    }

    public void UnregisterPuzzlePiece(PuzzlePiece piece)
    {
        foreach (var spawnSide in registeredPiecesLookup)
        {
            var registry = spawnSide.Value;
            if (!registry.ContainsKey(piece.identifier)) continue;
            registry.Remove(piece.identifier);
        }
    }

    //ToDo offset the item spawning to one side or the other or both. Variable delays?
    private void Update()
    {
        if (!canSpawn) return;

        timePassed += Time.deltaTime;
        if (timePassed < spawnDelay) return;
            
        SpawnPiece();
        timePassed = 0;
    }

    //ToDo Choose which side to spawn from
    private void SpawnPiece()
    {
        //Choose spawn point
        var nextSpawnSide = Random.value >= 0.5f ? SpawnSide.Left : SpawnSide.Right;
        var spawnPoint = nextSpawnSide == SpawnSide.Left ? leftSpawn : rightSpawn;
        if (!spawnPoint) return;
        
        //Choose piece to spawn
        var registry = registeredPiecesLookup[nextSpawnSide];
        var registryKeys = registry.Keys.ToArray();
        var index = Random.Range(0, registryKeys.Length);
        var pieceToSpawn = registry[registryKeys[index]];
        
        //Spawn piece
        Instantiate(pieceToSpawn, spawnPoint.position, Random.rotation);
    }

    public void DespawnPiece(PuzzlePiece piece, float despawnDelay = 0)
    {
        GameObject pieceToDespawn = null;
        foreach (var spawnedPiece in spawnedPieces)
        {
            if (spawnedPiece != piece.gameObject) continue;
            pieceToDespawn = spawnedPiece;
        }

        if (!pieceToDespawn) return;
        Destroy(pieceToDespawn, despawnDelay);
    }

    public void ResetSpawner()
    {
        StopSpawning();

        if (spawnedPieces == null ||
            spawnedPieces.Count == 0) return;
        
        foreach (var piece in new List<GameObject>(spawnedPieces))
        {
            if (!piece) continue;
            Destroy(piece.gameObject);
        }
		
        spawnedPieces.Clear();
    }
}

public enum SpawnSide
{
    Left, 
    Right
}
