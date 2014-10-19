using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public Collider doorCollider;

	private PathMovement pathMovement;

	[HideInInspector] public SpiderTankState returnState;

	public override void Awake()
	{
		base.Awake();

		pathMovement = GetComponent<PathMovement>();
		pathMovement.RegisterDestinationReachedCallback( new PathMovement.DesinationReached( DestinationReached ) );
	}

	public void OnEnable()
	{
		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
		pathMovement.enabled = true;
	}

	public void DestinationReached( PathMovement movement )
	{
		enabled = false;
		movement.enabled = false;
		movement.traverseBackwards = !movement.traverseBackwards;

		// re-enable collisions with the doorway.
		Physics.IgnoreCollision( collider, doorCollider, false );

		// transition to another state
		returnState.enabled = true;
	}
}
