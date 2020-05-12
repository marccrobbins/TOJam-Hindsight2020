using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
	public List<PuzzleSlot> slots;
	public bool puzzleCompleted = false;
	
    public void PreparePuzzle()
    {
	    if (slots == null ||
	        slots.Count == 0) return;
	    
		//prepare the slots
		foreach (var slot in slots)
		{
			slot.PrepareSlot(this);
		}
    }

    public void UpdatePuzzleProgress()
    {
	    if (slots == null ||
	        slots.Count == 0) return;	
	    
	    //Check puzzle progress here
	    foreach (var slot in slots)
	    {
		    var isSlotEmpty = !slot.isFilled;
		    if (isSlotEmpty) return;
	    }
	    
	    PuzzleDone();
    }

	private void PuzzleDone()
	{
		puzzleCompleted = true;
		GameManager.Instance.EndGame(puzzleCompleted);
	}

	public void ResetPuzzle()
	{
		if (slots == null ||
		    slots.Count == 0) return;

		foreach (var slot in slots)
		{
			if (!slot) continue;
			slot.ResetSlot();
		}
	}
}
