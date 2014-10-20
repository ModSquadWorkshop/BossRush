using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public Collider doorCollider;
	public float mainCanonCooldown;

	private PathMovement pathMovement;

	[HideInInspector] public SpiderTankState returnState;

	public override void Awake()
	{
		base.Awake();

		pathMovement = GetComponent<PathMovement>();
		pathMovement.RegisterDestinationReachedCallback( new PathMovement.DesinationReached( DestinationReached ) );
	}

	public void Update()
	{
		spiderTank.LookMainCanon();
		spiderTank.FireMainCanon();
	}

	public void OnEnable()
	{
		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
		pathMovement.enabled = true;
		mainCanon.SetCooldown( mainCanonCooldown );
	}

	public void OnDisable()
	{
		pathMovement.enabled = false;

		// re-enable collisions with the doorway.
		Physics.IgnoreCollision( collider, doorCollider, false );
	}

	public void DestinationReached( PathMovement movement )
	{
		enabled = false;
		pathMovement.traverseBackwards = !pathMovement.traverseBackwards;

		// transition to another state
		returnState.enabled = true;
	}
}
