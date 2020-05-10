using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
	[SerializeField] private PuzzleSlot SlotPrefab;
	public PuzzleAssemblyPiece[] Pieces;
	[SerializeField] private List<PuzzleSlot> Slots;
	private bool pleaseStopLogging = false;
	public bool PuzzleCompleted = false;
	
    public void PreparePuzzle()
    {
        foreach(PuzzleAssemblyPiece piece in Pieces)
		{
			piece.GrabMesh();
			PuzzleSlot newSlot = Instantiate(SlotPrefab, piece.transform.position, piece.transform.rotation, this.transform);
			piece.matchingSlot = newSlot;
			Slots.Add(newSlot);
			newSlot.StartTrackingPiece(piece);
		}
    }

    // Update is called once per frame
    void Update()
    {
		var isDone = true;
        foreach(var slot in Slots)
		{
			if(slot.gameObject.activeSelf)
			{
				isDone = false;
			}
		}
		if(isDone) Debug.Log("DONE? " + isDone);

		if(isDone && !pleaseStopLogging)
		{
			PuzzleDone();
		}
    }

	private void PuzzleDone()
	{
		pleaseStopLogging = true;
		PuzzleCompleted = true;
		GameManager.Instance.EndGame(PuzzleCompleted);
	}
}
