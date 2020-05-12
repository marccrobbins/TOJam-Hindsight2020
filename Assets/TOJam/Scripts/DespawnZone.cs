using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		//Destroy puzzle
		var puzzle = other.GetComponentInParent<Puzzle>();
		if (puzzle)
		{
			GameManager.Instance.TryAgain();
			Destroy(other.gameObject);
		}
		
		//Destroy puzzle piece
		var piece = other.GetComponent<PuzzlePiece>();
		if (piece) SpawnManager.Instance.DespawnPiece(piece);
	}
}
