using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PuzzlePiece piece = other.GetComponent<PuzzlePiece>();
		Puzzle puzz = other.GetComponent<Puzzle>();
		if(puzz != null)
		{
			GameManager.Instance.PuzzleDone(puzz.PuzzleCompleted);
			Destroy(other.gameObject);
		}
		else if (piece != null)
		{
			Destroy(other.gameObject);
		}
	}
}
