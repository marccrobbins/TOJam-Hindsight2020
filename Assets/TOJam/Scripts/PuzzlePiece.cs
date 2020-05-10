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

	private float snapInPlaceThreshold = 0.05f;
	private bool isMagnetizing = false;
	private PuzzleSlot magnetizingTarget = null;
	private float rotationSpeed = 0.00001f;
	private float magSpeed = 0.8f;

	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private MeshCollider meshCollider;

	private void Start()
	{
		
	}

	public void MatchPiece(PuzzleAssemblyPiece matchMe)
	{
		pieceName = matchMe.pieceName;
		transform.localScale = matchMe.transform.lossyScale;
		meshFilter.mesh = matchMe.GetComponent<MeshFilter>()?.sharedMesh;
		meshCollider.sharedMesh = meshFilter.sharedMesh;
		transform.rotation = UnityEngine.Random.rotation;
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

	public void StartMagnetizing(PuzzleSlot goToMe)
	{
		isMagnetizing = true;
		magnetizingTarget = goToMe;
		GetComponent<Collider>().enabled = false;
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		magSpeed = Vector3.Distance(transform.position, goToMe.transform.position) / 0.5f;
		rotationSpeed = Quaternion.Angle(transform.rotation, goToMe.transform.rotation) / 0.5f;
	}

	public void Update()
	{
		if (isMagnetizing)
		{
			if (Vector3.Distance(transform.position, magnetizingTarget.transform.position) < snapInPlaceThreshold)
			{
				magnetizingTarget.SnapInPlaceDone();
				gameObject.SetActive(false);
			}
			else
			{
				Vector3 newPos = Vector3.MoveTowards(transform.position, magnetizingTarget.transform.position, magSpeed * Time.deltaTime);

				Quaternion newRot;
				if ( Quaternion.Angle(transform.rotation, magnetizingTarget.transform.rotation) > 5)
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

	private void OnDestroy()
	{
		
	}
}
