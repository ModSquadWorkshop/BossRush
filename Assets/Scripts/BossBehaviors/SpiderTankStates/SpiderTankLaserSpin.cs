using UnityEngine;
using System.Collections;

public class SpiderTankLaserSpin : SpiderTankState
{
	public float rotation;
	public bool spawnMinions;

	public float duration;

	void OnEnable()
	{
		spawner.enabled = spawnMinions;
		Invoke( "TransitionOut", duration );
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void Update()
	{
		spiderTank.laserCanon.PerformPrimaryAttack();
	}

	void OnDisable()
	{
		spawner.enabled = false;
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void FixedUpdate()
	{
		Vector3 angularVelocity = rigidbody.angularVelocity;
		angularVelocity.y = rotation;
		rigidbody.angularVelocity = angularVelocity;
	}

	void TransitionOut()
	{
		enabled = false;

		if ( Random.value < 0.5f )
		{
			spiderTank.basicState.enabled = true;
		}
		else
		{
			spiderTank.turboState.enabled = true;
		}
	}
}
