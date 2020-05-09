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
		if (other.gameObject.CompareTag("Conveyer"))
		{
			other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position + (transform.forward * 1000), Velocity * Time.deltaTime);
		}
	}
}
