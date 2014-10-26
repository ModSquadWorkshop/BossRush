using UnityEngine;
using System.Collections;

public class SpiderTankEnterState : SpiderTankState
{
	public Transform destination;

	private MoveTowardsTarget _movement;

	public override void Awake()
	{
		base.Awake();

		_movement = GetComponent<MoveTowardsTarget>();
	}

	void OnEnable()
	{
		_movement.target = destination;
		_movement.enabled = true;

		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
	}

	void Update()
	{
		if ( ( transform.position - destination.position ).sqrMagnitude < 1.0f )
		{
			enabled = false;
			spiderTank.basicState.enabled = true;
		}
	}

	void OnDisable()
	{
		_movement.enabled = false;
		Physics.IgnoreCollision( collider, doorCollider, false );
	}
}
