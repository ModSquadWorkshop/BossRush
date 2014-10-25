using UnityEngine;
using System.Collections;

public class SpiderTankTurboState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float duration;

	public MoveTowardsTarget movementScript;

	public override void Awake()
	{
		base.Awake();
		movementScript.target = player;
	}

	void OnEnable()
	{
		spiderTank.mainCanon.SetCooldown( canonDelay );
		spawner.amountPerWave = amountPerWave;
		spawner.enabled = true;
		movementScript.enabled = true;

		// register for health trigger callbacks
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );

		Invoke( "TransitionOut", duration );
	}

	void Update()
	{
		spiderTank.LookAllGuns( turretSpeed );
		spiderTank.FireAllGuns();
	}

	void OnDisable()
	{
		spawner.enabled = false;
		movementScript.enabled = false;
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void TransitionOut()
	{
		enabled = false;

		if ( Random.value < 0.5f )
		{
			spiderTank.laserSpin.enabled = true;
		}
		else
		{
			spiderTank.basicState.enabled = true;
		}
	}
}
