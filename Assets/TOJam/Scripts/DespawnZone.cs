using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		//Destroy puzzle
		var puzz = other.GetComponent<Puzzle>();
		if(!puzz)
		{
			GameManager.Instance.TryAgain();
			Destroy(other.gameObject);
		}
		
		//Destroy puzzle piece
		var piece = other.GetComponent<PuzzlePiece>();
		if (!piece) Destroy(other.gameObject);
	}
}
