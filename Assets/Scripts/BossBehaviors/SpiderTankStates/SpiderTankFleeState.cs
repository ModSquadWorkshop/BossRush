using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public Collider doorCollider;
	public float mainCanonCooldown;

	public int minionsPerWave;

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
		mainCanon.SetCooldown( mainCanonCooldown );

		spawner.enabled = true;
		spawner.amountPerWave = minionsPerWave;
		spawner.StartSpawning();
	}

	public void Update()
	{
		spiderTank.LookMainCanon();
		spiderTank.FireMainCanon();
	}

	public void OnDisable()
	{
		spawner.enabled = false;
		pathMovement.enabled = false;
		spawner.StopSpawning();
	}

	public void DestinationReached( PathMovement movement )
	{
		enabled = false;
		pathMovement.traverseBackwards = !pathMovement.traverseBackwards;

		// re-enable collisions with the doorway.
		Physics.IgnoreCollision( collider, doorCollider, false );

		// transition to another state
		returnState.enabled = true;
	}
}
