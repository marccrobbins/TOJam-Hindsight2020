using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
	public string pieceName;
	[Range(1,100)] public float pieceCollisionSize;

	public VelocityEstimator velocityEstimator;
	public Rigidbody rigidbody;
	
	public bool isBeingHovered;

	private void Start()
	{
		
	}

	public void SetHoverState(bool isActive)
	{
		//Add some sort of visible cue that this piece is being hovered over by a picker
		
		isBeingHovered = isActive;
	}

	public void GotPickedUp(Transform parent)
	{
		if (rigidbody)
		{
			rigidbody.useGravity = false;
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
		
		transform.SetParent(parent);
	}

	public void GotDropped()
	{
		transform.SetParent(null);

		if (!rigidbody) return;
	
		rigidbody.useGravity = true;
		rigidbody.velocity = velocityEstimator ? velocityEstimator.Velocity : Vector3.zero;
		rigidbody.angularVelocity = velocityEstimator ? velocityEstimator.AngularVelocity : Vector3.zero;
	}
}
