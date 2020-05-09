using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZippyBot : MonoBehaviour
{
	[SerializeField] private Transform[] Waypoints;
	[SerializeField] private NavMeshAgent agent;

	private float waitTimeMin = 5f;
	private float waitTimeMax = 20f;

	private float waitTimer = 0f;

	private bool isOnPosSide = false;

    // Start is called before the first frame update
    void Start()
    {
		isOnPosSide = transform.position.x > 0;
		waitTimer = Random.Range(waitTimeMin, waitTimeMax);
	}

    // Update is called once per frame
    void Update()
    {
		waitTimer -= Time.deltaTime;
		if(waitTimer <= 0f)
		{
			waitTimer = Random.Range(waitTimeMin, waitTimeMax);
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		Puzzle puzz = other.GetComponent<Puzzle>();
		if (puzz != null)
		{
			foreach (PuzzleAssemblyPiece piece in puzz.Pieces)
			{
				if (!piece.gameObject.activeSelf) { 
					if ((isOnPosSide && piece.transform.position.x > 0)
					|| (!isOnPosSide && piece.transform.position.x < 0))
					{
						agent.SetDestination(piece.transform.position);
					}
				}
			}
		}
	}
}
