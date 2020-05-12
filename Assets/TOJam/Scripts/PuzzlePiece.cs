using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
	public event Action OnSnapToSlot;
	
	public string identifier;
	public VelocityEstimator velocityEstimator;
	public Rigidbody rigidbody;
	public Collider collision;
	
	public bool isBeingHovered;

	private float snapInPlaceThreshold = 0.05f;
	private bool isMagnetizing;
	private PuzzleSlot magnetizingTarget;
	private float rotationSpeed = 0.00001f;
	private float magSpeed = 0.8f;

	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private MeshCollider meshCollider;

	public void SetHoverState(bool isActive)
	{
		//Add some sort of visible cue that this piece is being hovered over by a picker
		isBeingHovered = isActive;
	}

	public void GotPickedUp(Transform parent)
	{
		transform.SetParent(parent);

		if (!rigidbody) return;
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	public void GotDropped()
	{
		transform.SetParent(null);

		if (!rigidbody) return;
	
		rigidbody.useGravity = true;
		rigidbody.velocity = velocityEstimator ? velocityEstimator.Velocity : Vector3.zero;
		rigidbody.angularVelocity = velocityEstimator ? velocityEstimator.AngularVelocity : Vector3.zero;
	}

	public void GotYanked()
	{
		transform.SetParent(null);
		if (collision) collision.enabled = true;
		rigidbody.useGravity = true;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}

	public void StartMagnetizing(PuzzleSlot target)
	{
		isMagnetizing = true;
		magnetizingTarget = target;
		if (collision) collision.enabled = false;
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		magSpeed = Vector3.Distance(transform.position, target.transform.position) / 0.5f;
		rotationSpeed = Quaternion.Angle(transform.rotation, target.transform.rotation) / 0.5f;
	}

	public void Update()
	{
		if (!isMagnetizing) return;
		if (Vector3.Distance(transform.position, magnetizingTarget.transform.position) < snapInPlaceThreshold)
		{
			isMagnetizing = false;
			OnSnapToSlot?.Invoke();
			transform.SetParent(magnetizingTarget.transform);
		}
		else
		{
			var newPos = Vector3.MoveTowards(transform.position, magnetizingTarget.transform.position,
				magSpeed * Time.deltaTime);

			Quaternion newRot;
			if (Quaternion.Angle(transform.rotation, magnetizingTarget.transform.rotation) > 5)
			{
				newRot = Quaternion.RotateTowards(
					transform.rotation, magnetizingTarget.transform.rotation, rotationSpeed * Time.deltaTime);
			}
			else
			{
				newRot = magnetizingTarget.transform.rotation;
			}

			transform.SetPositionAndRotation(newPos, newRot);
		}
	}
}
