using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZippyBot : MonoBehaviour
{
	[SerializeField] private Transform[] Waypoints;
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private PuzzlePiece piecePrefab;

	private float waitTimeMin = 5f;
	private float waitTimeMax = 20f;

	[SerializeField] private float waitTimer = 0f;

	private bool isOnPosSide = false;

	private Vector3 originalPosition;

	private float reachedWaypointThreshold = 0.4f;
	private float reachedPieceThreshold = 1.0f;

	private bool isGoingOutToPlay = false;

	private State currentState = State.Waiting;

	private PuzzleAssemblyPiece targetPiece;

	private float yankForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
		isOnPosSide = transform.position.x > 0;
		waitTimer = Random.Range(waitTimeMin, waitTimeMax);
		originalPosition = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
		switch(currentState)
		{
			case State.Waiting:
				waitTimer -= Time.deltaTime;
				if (waitTimer <= 0f)
				{
					agent.SetDestination(Waypoints[Random.Range(0, Waypoints.Length - 1)].position);
					waitTimer = Random.Range(waitTimeMin, waitTimeMax);
					currentState = State.Searching;
				}
				break;
			case State.Searching:
				if (Vector3.Distance(transform.position, agent.destination) < reachedWaypointThreshold)
				{
					agent.SetDestination(originalPosition);
					currentState = State.Returning;
				}
				break;
			case State.Chasing:
				if (Vector3.Distance(transform.position, agent.destination) < reachedPieceThreshold)
				{
					PullOutPiece();
				}
				break;
			case State.Returning:
				if (Vector3.Distance(transform.position, agent.destination) < reachedWaypointThreshold)
				{
					currentState = State.Waiting;
				}
				break;
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
						targetPiece = piece;
						agent.SetDestination(piece.transform.position);
						currentState = State.Chasing;
					}
				}
			}
		}
	}

	private void PullOutPiece()
	{
		agent.SetDestination(originalPosition);
		currentState = State.Returning;

		Vector3 yank = targetPiece.transform.position - transform.position;
		yank = yank.normalized * yankForce;
		yank += Vector3.up * yankForce / 4f;

		targetPiece.matchingSlot.IveBeenYanked();

		PuzzlePiece throwMe = Instantiate(piecePrefab, targetPiece.transform.position + yank.normalized, Random.rotation);
		throwMe.rigidbody.AddForce(yank);
	}

	enum State
	{
		Waiting,
		Searching,
		Chasing,
		Pulling,
		Returning
	}
}
