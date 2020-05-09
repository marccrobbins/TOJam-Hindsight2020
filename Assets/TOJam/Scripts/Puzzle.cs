using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
	[SerializeField] private PuzzleSlot SlotPrefab;
	[SerializeField] private PuzzleAssemblyPiece[] Pieces;
	[SerializeField] private List<PuzzleSlot> Slots;
	private bool pleaseStopLogging = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PuzzleAssemblyPiece piece in Pieces)
		{
			PuzzleSlot newSlot = Instantiate(SlotPrefab, piece.transform.position, piece.transform.rotation, this.transform);
			Slots.Add(newSlot);
			newSlot.StartTrackingPiece(piece);
		}
    }

    // Update is called once per frame
    void Update()
    {
		bool isDone = true;
        foreach(PuzzleSlot slot in Slots)
		{
			if(slot.gameObject.activeSelf)
			{
				isDone = false;
			}
		}

		if(isDone && !pleaseStopLogging)
		{
			PuzzleDone();
		}
    }

	private void PuzzleDone()
	{
		Debug.Log("YA DONE");
		Debug.LogWarning("YA DONE");
		Debug.LogError("YA DONE");
		pleaseStopLogging = true;
	}
}
