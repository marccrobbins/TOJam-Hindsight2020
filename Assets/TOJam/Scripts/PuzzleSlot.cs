using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
	public PuzzleAssemblyPiece matchingPiece;
	public SphereCollider sphere;
	public MeshRenderer hologramRenderer;

	public void PrepareSlot()
	{
		//Send piece to spawnmanager
	}
	
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

	public bool AmIThisPiece(string name)
	{
		return matchingPiece.pieceName == name;
	}

	public void IveBeenYanked()
	{
		matchingPiece.gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
}
