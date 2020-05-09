using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
	private PuzzlePiece matchingPiece;
	[SerializeField] private SphereCollider sphere;

	public void StartTrackingPiece(PuzzlePiece pieceToEnable)
	{
		matchingPiece = pieceToEnable;
		sphere.radius = matchingPiece.pieceCollisionSize / 200f;
		matchingPiece.gameObject.SetActive(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		PuzzlePiece piece = other.GetComponent<PuzzlePiece>();
		if (piece == null) return;

		if(matchingPiece.pieceName == piece.pieceName)
		{
			piece.gameObject.SetActive(false);
			matchingPiece.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
