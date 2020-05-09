using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
	private PuzzleAssemblyPiece matchingPiece;
	[SerializeField] private SphereCollider sphere;

	public void StartTrackingPiece(PuzzleAssemblyPiece pieceToEnable)
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
			piece.StartMagnetizing(this);

		}
	}

	public void SnapInPlaceDone()
	{
		sphere.enabled = false;
		matchingPiece.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
