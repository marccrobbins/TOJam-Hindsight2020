using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
	public string pieceName;
	[Range(1,100)] public float pieceCollisionSize;

	public bool isBeingHovered;
	
	public void SetHoverState(bool isActive)
	{
		//Add some sort of visible cue that this piece is being hovered over by a picker
		
		isBeingHovered = isActive;
	}
}
