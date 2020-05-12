using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
	[SerializeField] private Transform CollisionRef;
	/// <summary>
	/// Velocity in m/s
	/// </summary>
	[SerializeField] private float Speed;
	private float speedMod = 1.0f;

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.root.gameObject.CompareTag("Conveyer"))
		{
			other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position + (transform.forward * 1000), Speed * speedMod * Time.deltaTime);
		}
	}

	public void SpeedUp()
	{
		speedMod = 20f;
	}
	
	public void SlowDown()
	{
		speedMod = 1f;
	}
}
