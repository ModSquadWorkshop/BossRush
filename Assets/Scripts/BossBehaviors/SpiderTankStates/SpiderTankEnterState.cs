using UnityEngine;
using System.Collections;

public class SpiderTankEnterState : SpiderTankState
{
	public Transform dropPoint;

	public override void OnEnable()
	{
		base.OnEnable();

		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
	}


	public override void OnDisable()
	{
		base.OnDisable();

		spiderTank.basicState.enabled = true;
		Physics.IgnoreCollision( collider, doorCollider, false );
	}
}
