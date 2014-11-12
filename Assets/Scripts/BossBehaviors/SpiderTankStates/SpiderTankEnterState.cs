using UnityEngine;
using System.Collections;

public class SpiderTankEnterState : SpiderTankState
{
	public EnterStateSettings enterStateSettings;

	private MoveTowardsTarget _movement;

	public override void Awake()
	{
		base.Awake();

		_movement = GetComponent<MoveTowardsTarget>();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_movement.target = enterStateSettings.destination;
		_movement.enabled = true;

		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
	}

	void Update()
	{
		enabled = ( transform.position - enterStateSettings.destination.position ).sqrMagnitude > 1.0f;
	}

	public override void OnDisable()
	{
		base.OnDisable();

		_movement.enabled = false;
		spiderTank.basicState.enabled = true;
		Physics.IgnoreCollision( collider, doorCollider, false );
	}
}


[System.Serializable]
public class EnterStateSettings
{
	public Transform destination;
}
