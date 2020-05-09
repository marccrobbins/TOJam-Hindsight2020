using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
	[SerializeField] private Transform CollisionRef;
	/// <summary>
	/// Velocity in m/s
	/// </summary>
	[SerializeField] private float Velocity;

	private void OnTriggerStay(Collider other)
	{
		other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position + (transform.forward * 1000), Velocity * Time.deltaTime);
		/*
		Debug.Log("Triggerstay! "+other.gameObject.name);
		Rigidbody body = other.attachedRigidbody;
		Vector3 forcePoint = other.ClosestPoint(CollisionRef.position);
		Vector3 velocityAdd = this.transform.forward * Velocity * Time.deltaTime;
		body.AddForceAtPosition(velocityAdd, forcePoint, ForceMode.VelocityChange);
		*/
	}
}
