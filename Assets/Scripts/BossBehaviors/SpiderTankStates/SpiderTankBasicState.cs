using UnityEngine;
using System.Collections;

public class SpiderTankBasicState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float minRushInterval, maxRushInterval;

	[Range( 0.0f, 1.0f )]
	public float turboChance;

	public PhysicsMovement movementScript;

	public override void OnEnable()
	{
		base.OnEnable();

		spiderTank.mainCanon.SetCooldown( canonDelay );

		// set initial states of movement scripts
		movementScript.enabled = true;
		spiderTank.rushState.returnState = this;

		// queue up first rush attack
		Invoke( "TransitionOut", Random.Range( minRushInterval, maxRushInterval ) );

		// register for health trigger callbacks
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void Update()
	{
		spiderTank.LookMainCanon( turretSpeed );
		spiderTank.FireMainCanon();

		Quaternion lookRotation = Quaternion.LookRotation( player.position - transform.position );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, 90.0f * Time.deltaTime );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		movementScript.enabled = false;
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void TransitionOut()
	{
		enabled = false;

		if ( Random.Range( 0.0f, 1.0f ) < turboChance )
		{
			spiderTank.turboState.enabled = true;
		}
		else
		{
			spiderTank.rushState.enabled = true;
		}
	}
}
