using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAssemblyPiece : MonoBehaviour
 {
	public string pieceName;
	[Range(1, 100)] public float pieceCollisionSize;
	public Mesh refMesh;
	[HideInInspector] public PuzzleSlot matchingSlot;

	public void GrabMesh()
	{
		refMesh = GetComponent<MeshFilter>()?.mesh;
	}
}
