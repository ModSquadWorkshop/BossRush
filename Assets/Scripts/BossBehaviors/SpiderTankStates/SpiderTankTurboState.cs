using UnityEngine;
using System.Collections;

public class SpiderTankTurboState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float duration;

	public MoveTowardsTarget movementScript;

	private FlankingSpawner _spawner;

	public override void Awake()
	{
		base.Awake();

		_spawner = GetComponent<FlankingSpawner>();
		movementScript.target = player;
	}

	void OnEnable()
	{
		spiderTank.mainCanon.SetCooldown( canonDelay );
		_spawner.enabled = true;
		_spawner.amountPerWave = amountPerWave;
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
		_spawner.enabled = false;
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
