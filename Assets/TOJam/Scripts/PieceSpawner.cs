﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
	[SerializeField] private PuzzlePiece PiecePrefab;
	private List<PuzzleAssemblyPiece> ReferencePieces;
	private bool isSpawning = false;
	public float TimeBetweenPieces = 1f;
	private float currentSpawnTimer = 0f;

	private List<GameObject> spawnedPieces = new List<GameObject>();

	public void StartSpawning(PuzzleAssemblyPiece[] refPieces)
	{
		ReferencePieces = new List<PuzzleAssemblyPiece>( refPieces);
		ReferencePieces.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
		isSpawning = true;
		currentSpawnTimer = TimeBetweenPieces;
	}
	public void StopSpawning()
	{
		isSpawning = false;
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

	public void Update()
	{
		if (isSpawning)
		{
			currentSpawnTimer -= Time.deltaTime;
			if (currentSpawnTimer <= 0f)
			{
				SpawnPiece();
				currentSpawnTimer = TimeBetweenPieces;
			}
		}
	}

	private void SpawnPiece()
	{
		var newPiece = Instantiate(PiecePrefab, transform.position, transform.rotation);
		spawnedPieces.Add(newPiece.gameObject);
		
		var matchMe = ReferencePieces[0];
		ReferencePieces.RemoveAt(0);

		newPiece.MatchPiece(matchMe);
		ReferencePieces.Add(matchMe);
	}
}
