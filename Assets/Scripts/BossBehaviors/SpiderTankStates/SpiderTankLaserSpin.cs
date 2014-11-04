using UnityEngine;
using System.Collections;

public class SpiderTankLaserSpin : SpiderTankState
{
	public float rotation;
	public float duration;

	public override void OnEnable()
	{
		base.OnEnable();

		Invoke( "TransitionOut", duration );
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void Update()
	{
		for ( int i = 0; i < spiderTank.laserCanon.Length; ++i )
		{
			spiderTank.laserCanon[i].PerformPrimaryAttack();
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();

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
