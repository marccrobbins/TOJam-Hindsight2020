using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
	public SpawnSide spawnSide;
	public PuzzlePiece matchingPiece;
	public SphereCollider collision;
	public MeshRenderer hologramRenderer;
	
	public bool isFilled { get; private set; }
	public PuzzlePiece slottedPiece { get; private set; }

	private Puzzle puzzle;
	
	public void PrepareSlot(Puzzle puzzle)
	{
		if (!matchingPiece) return;
		
		this.puzzle = puzzle;
		matchingPiece.OnSnapToSlot += PieceSnappedInPlace;
		SpawnManager.Instance.RegisterPuzzlePiece(matchingPiece, spawnSide);
	}

	public void OnTriggerEnter(Collider other)
	{
		var piece = other.GetComponent<PuzzlePiece>();
		if (!piece) return;

		//Compare piece
		if (IsCorrectPiece(piece))
		{
			slottedPiece = piece;
			slottedPiece.StartMagnetizing(this);
		}
		else
		{
			//Blow it away with physics then destroy it
			piece.rigidbody.AddForce(Vector3.up * 100);
			SpawnManager.Instance.DespawnPiece(piece, 5);
		}
	}

	//Called when piece is pulled from the puzzle
	private void OnTriggerExit(Collider other)
	{
		var piece = other.GetComponent<PuzzlePiece>();
		if (!piece) return;
		
		//Tell the puzzle this slot has been emptied
		if (collision) collision.enabled = true;
		if (hologramRenderer) hologramRenderer.enabled = true;
		puzzle.UpdatePuzzleProgress();
	}

	private bool IsCorrectPiece(PuzzlePiece piece)
	{
		return string.Equals(piece.identifier, matchingPiece.identifier);
	}

	private void PieceSnappedInPlace()
	{
		if (collision) collision.enabled = false;
		if (hologramRenderer) hologramRenderer.enabled = false;
		isFilled = true;
		puzzle.UpdatePuzzleProgress();
	}
	
	public void ResetSlot()
	{
		SpawnManager.Instance.UnregisterPuzzlePiece(matchingPiece);
	}
}
