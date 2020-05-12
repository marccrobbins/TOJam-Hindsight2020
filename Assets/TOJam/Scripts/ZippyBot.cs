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

	[SerializeField] private float waitTimer;

	private bool isOnPosSide;
	private Vector3 originalPosition;
	private float reachedWaypointThreshold = 0.4f;
	private float reachedPieceThreshold = 1.0f;
	private bool isGoingOutToPlay = false;
	private State currentState = State.Waiting;
	private PuzzlePiece targetPiece;
	private float yankForce = 100f;

    private void Start()
    {
		isOnPosSide = transform.position.x > 0;
		waitTimer = Random.Range(waitTimeMin, waitTimeMax);
		originalPosition = transform.position;
	}

    private void Update()
    {
	    //Dear god dude, use a state machine they take like 5 minutes to build! lmao
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
	    var puzzle = other.GetComponent<Puzzle>();
	    if (!puzzle) return;

	    foreach (var slot in puzzle.slots)
	    {
		    if (!slot.isFilled) continue;
		    if (isOnPosSide && slot.transform.position.x > 0
		        || !isOnPosSide && slot.transform.position.x < 0)
		    {
			    targetPiece = slot.slottedPiece;
			    agent.SetDestination(slot.transform.position);
			    currentState = State.Chasing;
		    }
	    }
    }

    private void PullOutPiece()
	{
		agent.SetDestination(originalPosition);
		currentState = State.Returning;

		var yank = targetPiece.transform.position - transform.position;
		yank = yank.normalized * yankForce;
		yank += yankForce / 4f * Vector3.up;

		targetPiece.GotYanked();

		var throwMe = Instantiate(piecePrefab, targetPiece.transform.position + yank.normalized, Random.rotation);
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
